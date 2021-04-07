using System;
using System.Collections;
using CsharpVoxReader;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Valve.VR;

public struct Clue
{
    public Clue(bool blank, int voxelCount, int gapCount)
    {
        Blank = blank;
        VoxelCount = voxelCount;
        GapCount = gapCount;
        Complete = false;
    }

    public bool Blank;
    public int VoxelCount;
    public int GapCount;
    public bool Complete;
}

[Serializable]
public struct VoxelInfo
{
    public VoxelInfo(VoxelManager.VoxelState voxelState, Color voxelColor, Vector3Int position)
    {
        VoxelState = voxelState;
        VoxelColor = voxelColor;
        Position = position;
    }

    public VoxelManager.VoxelState VoxelState;
    public Color VoxelColor;
    public Vector3Int Position;
}

public class VoxelManager : MonoBehaviour
{
    public enum GameMode
    {
        Build,
        Destroy,
        Mark,
    }

    public enum VoxelState
    {
        Unmarked = 0,
        Marked = 1,
        Cleared = 2,
    }

    [SerializeField] private Puzzle puzzleObject;

    [Header("Random Generation")] 
    [SerializeField] private bool generateRandomPuzzle;
    [SerializeField] private int length, height, width; // length = x, height = y, width = z 
    
    [Header("Settings")]
    [SerializeField] private float rotateSpeed;

    [Header("Dependencies")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject cube;

    [SerializeField] private Material _clearMaterial;

    private int _visibleLayersX, _visibleLayersY, _visibleLayersZ;
    private GameObject[,,] _voxels;
    private Clue[,] _frontClues, _sideClues, _topClues;
    private VoxelState[,,] _solution;
    private Vector3 _target = Vector3.zero;
    private Transform _cameraTransform;
    private bool _coroutineFinished = true;
    private bool _canVerticallyRotate = true;
    private VoxelInfo[,,] _puzzle;

    private GameMode _currentGameMode = GameMode.Mark;
    [SerializeField] private Text _modeText;

    public GameMode CurrentGameMode
    {
        get => _currentGameMode;
        set => _currentGameMode = value;
    }

    private void Start()
    {
        _target = transform.position;
        
        _visibleLayersX = length - 1;
        _visibleLayersY = height - 1;
        _visibleLayersZ = width - 1;

        GameObject loadedPuzzle = GameObject.FindGameObjectWithTag("PuzzleLoader");
        if (loadedPuzzle != null)
        {
            puzzleObject = loadedPuzzle.GetComponent<StartPuzzle>().PuzzleObject;
        }
        
        if (generateRandomPuzzle || puzzleObject == null)
        {
            CreateSolution();
        }
        else
        {
            _puzzle = SetPuzzleVoxels(
                puzzleObject.Palette, 
                puzzleObject.Data, 
                puzzleObject.SizeX, 
                puzzleObject.SizeY, 
                puzzleObject.SizeZ);
            _solution = GetSolutionVoxelStates(_puzzle);
            length = _solution.GetLength(0);
            height = _solution.GetLength(1);
            width = _solution.GetLength(2);
        }
        InitializeVoxels();

        _cameraTransform = mainCamera.transform;

        _frontClues = new Clue[length, height];
        _sideClues = new Clue[width, height];
        _topClues = new Clue[length, width];

        NumberAllVoxels();

        // VoxelState[,,] calculatedSolution = Validator.IsValid(_solution, _frontClues, _sideClues, _topClues);
        //
        // Debug.Log("Calculated Sol:");
        // PrintSolution(calculatedSolution);

        _modeText.text = "Mark";
    }

    public void UpdateAdjacentVoxelHints(Vector3 indexPosition)
    {
        int i = (int) indexPosition.x;
        int j = (int) indexPosition.y;
        int k = (int) indexPosition.z;

        if (!_frontClues[i, j].Blank) // if the clues in the line aren't blank
        {
            // Update voxel in front
            if (k > 0) // if the voxel is not a boundary
            {
                Voxel voxelToUpdate = _voxels[i, j, k - 1].GetComponent<Voxel>();

                if (voxelToUpdate.IsVisible) // if the voxel is visible to the player, then update its hint
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Rear].SetHintText(
                        _frontClues[i, j].VoxelCount,
                        _frontClues[i, j].GapCount);
                }
                else
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Rear].ClearHintText();
                }
            }

            // Update voxel in rear
            if (k < width - 1) // if the voxel is not a boundary
            {
                Voxel voxelToUpdate = _voxels[i, j, k + 1].GetComponent<Voxel>();

                if (voxelToUpdate.IsVisible) // if the voxel is visible to the player, then update its hint
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Front].SetHintText(
                        _frontClues[i, j].VoxelCount,
                        _frontClues[i, j].GapCount);
                }
                else
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Front].ClearHintText();
                }
            }
        }
        
        if (!_sideClues[k, j].Blank) // if the clues in the line aren't blank
        {
            // Update voxel to the left
            if (i > 0) // if the voxel is not a boundary
            {
                Voxel voxelToUpdate = _voxels[i - 1, j, k].GetComponent<Voxel>();

                if (voxelToUpdate.IsVisible) // if the voxel is visible to the player, then update its hint
                {
                    voxelToUpdate.Hints[(int) VoxelSide.RightSide].SetHintText(
                        _sideClues[k, j].VoxelCount,
                        _sideClues[k, j].GapCount);
                }
                else
                {
                    voxelToUpdate.Hints[(int) VoxelSide.RightSide].ClearHintText();
                }
            }
            
            // Update voxel to the right
            if (i < length - 1) // if the voxel is not a boundary
            {
                Voxel voxelToUpdate = _voxels[i + 1, j, k].GetComponent<Voxel>();

                if (voxelToUpdate.IsVisible) // if the voxel is visible to the player, then update its hint
                {
                    voxelToUpdate.Hints[(int) VoxelSide.LeftSide].SetHintText(
                        _sideClues[k, j].VoxelCount,
                        _sideClues[k, j].GapCount);
                }
                else
                {
                    voxelToUpdate.Hints[(int) VoxelSide.LeftSide].ClearHintText();
                }
            }
        }

        if (!_topClues[i, k].Blank) // if the clues in the line aren't blank
        {
            // Update voxel below
            if (j > 0) // if the voxel is not a boundary
            {
                Voxel voxelToUpdate = _voxels[i, j - 1, k].GetComponent<Voxel>();

                if (voxelToUpdate.IsVisible) // if the voxel is visible to the player, then update its hint
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Top].SetHintText(
                        _topClues[i, k].VoxelCount,
                        _topClues[i, k].GapCount);
                }
                else
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Top].ClearHintText();
                }
            }
            
            // Update voxel above
            if (j < height - 1) // if the voxel is not a boundary
            {
                Voxel voxelToUpdate = _voxels[i, j + 1, k].GetComponent<Voxel>();

                if (voxelToUpdate.IsVisible) // if the voxel is visible to the player, then update its hint
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Bottom].SetHintText(
                        _topClues[i, k].VoxelCount,
                        _topClues[i, k].GapCount);
                }
                else
                {
                    voxelToUpdate.Hints[(int) VoxelSide.Bottom].ClearHintText();
                }
            }
        }
    }

    private void InitializeVoxels()
    {
        _voxels = new GameObject[length, height, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    _voxels[i, j, k] = Instantiate(cube, transform.position + new Vector3((i - length/2) * cube.transform.localScale.x, (j - height/2) * cube.transform.localScale.y, (k - width/2) * cube.transform.localScale.z),
                        Quaternion.identity, transform);
                    _voxels[i, j, k].GetComponent<Voxel>().IsPuzzleVoxel = _solution[i, j, k] == VoxelState.Marked;
                    _voxels[i, j, k].GetComponent<Voxel>().Manager = this;
                    _voxels[i, j, k].GetComponent<Voxel>().IndexPosition = new Vector3(i, j, k);
                }
            }
        }
    }

    private void Update()
    {
        ManageRotations();
        ManageVisibleLayers();
        ManageMode();

        // TEMP TEMP TEMP
        bool performAction = SteamVR_Actions.picross.PerformAction[SteamVR_Input_Sources.Any].stateDown;

        bool grabLayer = SteamVR_Actions.picross.GrabLayer[SteamVR_Input_Sources.Any].stateDown;
    }

    private void ManageMode()
    {

        bool switchMode = SteamVR_Actions.picross.SwitchMode[SteamVR_Input_Sources.Any].stateDown;

        if (switchMode && _currentGameMode == GameMode.Mark)
        {
            _currentGameMode = GameMode.Build;
            MakeBuildable();
            _modeText.text = "Build";
        } else if (switchMode && _currentGameMode == GameMode.Build)
        {
            _currentGameMode = GameMode.Destroy;
            MakeDestroyable();
            _modeText.text = "Destroy";
        } else if (switchMode && _currentGameMode == GameMode.Destroy)
        {
            _currentGameMode = GameMode.Mark;
            MakeMarkable();
            _modeText.text = "Mark";
        }
 
        GameMode newGameMode;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            newGameMode = GameMode.Build;
            if (_currentGameMode != newGameMode)
            {
                _currentGameMode = newGameMode;
                MakeBuildable();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            newGameMode = GameMode.Destroy;
            if (_currentGameMode != newGameMode)
            {
                _currentGameMode = newGameMode;
                MakeDestroyable();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            newGameMode = GameMode.Mark;
            if (_currentGameMode != newGameMode)
            {
                _currentGameMode = newGameMode;
                MakeMarkable();
            }
        }
    }

    private void MakeBuildable()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    GameObject currentVoxel = _voxels[i, j, k];
                    if (!currentVoxel.GetComponent<Voxel>().IsVisible)
                    {
                        currentVoxel.SetActive(true);
                        currentVoxel.GetComponent<MeshRenderer>().material = _clearMaterial;
                        currentVoxel.GetComponent<Voxel>().IsHovering = false;
                    }
                }
            }
        }
    }

    private void MakeDestroyable()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    GameObject currentVoxel = _voxels[i, j, k];
                    if (!currentVoxel.GetComponent<Voxel>().IsVisible)
                    {
                        currentVoxel.SetActive(false);
                        currentVoxel.GetComponent<Voxel>().IsHovering = false;
                    }
                }
            }
        }
    }

    private void MakeMarkable()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    GameObject currentVoxel = _voxels[i, j, k];
                    if (!currentVoxel.GetComponent<Voxel>().IsVisible)
                    {
                        currentVoxel.SetActive(false);
                        currentVoxel.GetComponent<Voxel>().IsHovering = false;
                    }
                }
            }
        }
    }

    private void CreateSolution()
    {
        _solution = new VoxelState[length, height, width];

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    _solution[i, j, k] = Convert.ToBoolean(Random.Range(0, 2)) ? VoxelState.Cleared : VoxelState.Marked;
                }
            }
        }

        Debug.Log("Actual Sol:");
        PrintSolution(_solution);
    }

    private void PrintSolution(VoxelState[,,] solution)
    {
        string matrices = "Solution:";
        for (int k = width - 1; k >= 0; k--)
        {
            matrices += $"\n Layer {k + 1}: \n";
            for (int j = height - 1; j >= 0; j--)
            {
                matrices += "| ";
                for (int i = 0; i < length; i++)
                {
                    char toAdd = 'u';
                    if (solution[i, j, k] != VoxelState.Unmarked)
                        toAdd = solution[i, j, k] == VoxelState.Marked ? '1' : '0';
                    matrices += $"{toAdd} ";
                }

                matrices += "|\n";
            }
        }

        Debug.Log(matrices);
    }

    private void NumberAllVoxels()
    {
        // Populate front clues
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < length; i++)
            {
                int voxelCount = 0;
                int gapCount = 0;
                bool previousWasGap = false;

                for (int k = 0; k < width; k++)
                {
                    if (_solution[i, j, k] == VoxelState.Marked)
                    {
                        voxelCount++;
                        previousWasGap = false;
                    }
                    else if (k != width - 1 && voxelCount > 0 && !previousWasGap)
                    {
                        gapCount++;
                        previousWasGap = true;
                    }

                    if (k == width - 1 && voxelCount <= 1)
                    {
                        gapCount = 0;
                    }
                }

                _frontClues[i, j] = new Clue(blank: false, voxelCount: voxelCount, gapCount: gapCount);
                _voxels[i, j, 0].GetComponent<Voxel>().SetSideText(VoxelSide.Front, voxelCount, gapCount);
                _voxels[i, j, width - 1].GetComponent<Voxel>().SetSideText(VoxelSide.Rear, voxelCount, gapCount);
            }
        }

        // Populate side clues
        for (int j = 0; j < height; j++)
        {
            for (int k = 0; k < width; k++)
            {
                int voxelCount = 0;
                int gapCount = 0;
                bool previousWasGap = false;

                for (int i = 0; i < length; i++)
                {
                    if (_solution[i, j, k] == VoxelState.Marked)
                    {
                        voxelCount++;
                        previousWasGap = false;
                    }
                    else if (i != length - 1 && voxelCount > 0 && !previousWasGap)
                    {
                        gapCount++;
                        previousWasGap = true;
                    }

                    if (i == length - 1 && voxelCount <= 1)
                    {
                        gapCount = 0;
                    }
                }

                _sideClues[k, j] = new Clue(blank: false, voxelCount: voxelCount, gapCount: gapCount);
                _voxels[0, j, k].GetComponent<Voxel>().SetSideText(VoxelSide.LeftSide, voxelCount, gapCount);
                _voxels[length - 1, j, k].GetComponent<Voxel>().SetSideText(VoxelSide.RightSide, voxelCount, gapCount);
            }
        }

        // populate top clues
        for (int k = 0; k < width; k++)
        {
            for (int i = 0; i < length; i++)
            {
                int voxelCount = 0;
                int gapCount = 0;
                bool previousWasGap = false;
                bool gap = false;

                for (int j = 0; j < height; j++)
                {
                    if (_solution[i, j, k] == VoxelState.Marked)
                    {
                        voxelCount++;
                        if (previousWasGap)
                        {
                            gap = true;
                        }

                        previousWasGap = false;
                    }
                    else if (j != height - 1 && voxelCount > 0 && !previousWasGap)
                    {
                        gapCount++;
                        previousWasGap = true;
                    }

                    if (j == height - 1 && !gap)
                    {
                        gapCount = 0;
                    }
                }

                _topClues[i, k] = new Clue(blank: false, voxelCount: voxelCount, gapCount: gapCount);
                _voxels[i, 0, k].GetComponent<Voxel>().SetSideText(VoxelSide.Bottom, voxelCount, gapCount);
                _voxels[i, height - 1, k].GetComponent<Voxel>().SetSideText(VoxelSide.Top, voxelCount, gapCount);
            }
        }

        string frontCluesString = "front clues:\n";
        for (int j = height - 1; j >= 0; j--)
        {
            frontCluesString += "| ";
            for (int i = length - 1; i >= 0; i--)
            {
                frontCluesString += $"({_frontClues[i, j].VoxelCount}^{_frontClues[i, j].GapCount}) ";
            }

            frontCluesString += "|\n";
        }

        Debug.Log(frontCluesString);

        string sideCluesString = "side clues:\n";
        for (int j = height - 1; j >= 0; j--)
        {
            sideCluesString += "| ";
            for (int k = width - 1; k >= 0; k--)
            {
                sideCluesString += $"({_sideClues[k, j].VoxelCount}^{_sideClues[k, j].GapCount}) ";
            }

            sideCluesString += "|\n";
        }

        Debug.Log(sideCluesString);

        string topCluesString = "top clues:\n";
        for (int k = 0; k < width; k++)
        {
            topCluesString += "| ";
            for (int i = length - 1; i >= 0; i--)
            {
                topCluesString += $"({_topClues[i, k].VoxelCount}^{_topClues[i, k].GapCount}) ";
            }

            topCluesString += "|\n";
        }

        Debug.Log(topCluesString);
    }

    private void ManageVisibleLayers()
    {
        if (_coroutineFinished)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && _visibleLayersX > 0)
            {
                for (int i = 0; i < _visibleLayersY + 1; i++)
                {
                    for (int j = 0; j < _visibleLayersZ + 1; j++)
                    {
                        ChangeVoxelVisible(_voxels[_visibleLayersX, i, j], false);
                        // voxels[visibleLayersX, i, j].SetActive(false);
                    }
                }

                _visibleLayersX--;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && _visibleLayersX < length - 1)
            {
                _visibleLayersX++;
                for (int i = 0; i < _visibleLayersY + 1; i++)
                {
                    for (int j = 0; j < _visibleLayersZ + 1; j++)
                    {
                        ChangeVoxelVisible(_voxels[_visibleLayersX, i, j], true);
                        // voxels[visibleLayersX, i, j].SetActive(true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && _visibleLayersY > 0)
            {
                for (int i = 0; i < _visibleLayersX + 1; i++)
                {
                    for (int j = 0; j < _visibleLayersZ + 1; j++)
                    {
                        ChangeVoxelVisible(_voxels[i, _visibleLayersY, j], false);
                        // voxels[i, visibleLayersY, j].SetActive(false);
                    }
                }

                _visibleLayersY--;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && _visibleLayersY < height - 1)
            {
                _visibleLayersY++;
                for (int i = 0; i < _visibleLayersX + 1; i++)
                {
                    for (int j = 0; j < _visibleLayersZ + 1; j++)
                    {
                        ChangeVoxelVisible(_voxels[i, _visibleLayersY, j], true);
                        // voxels[i, visibleLayersY, j].SetActive(true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha5) && _visibleLayersZ > 0)
            {
                for (int i = 0; i < _visibleLayersX + 1; i++)
                {
                    for (int j = 0; j < _visibleLayersY + 1; j++)
                    {
                        ChangeVoxelVisible(_voxels[i, j, _visibleLayersZ], false);
                        // voxels[i, j, visibleLayersZ].SetActive(false);
                    }
                }

                _visibleLayersZ--;
            }

            if (Input.GetKeyDown(KeyCode.Alpha6) && _visibleLayersZ < width - 1)
            {
                _visibleLayersZ++;
                for (int i = 0; i < _visibleLayersX + 1; i++)
                {
                    for (int j = 0; j < _visibleLayersY + 1; j++)
                    {
                        ChangeVoxelVisible(_voxels[i, j, _visibleLayersZ], true);
                        // voxels[i, j, visibleLayersZ].SetActive(true);
                    }
                }
            }
        }
    }

    private void ChangeVoxelVisible(GameObject voxel, bool isActivated)
    {
        if (isActivated)
        {
            _coroutineFinished = false;
            StartCoroutine(GrowVoxel(voxel));
        }
        else
        {
            _coroutineFinished = false;
            StartCoroutine(ShrinkVoxel(voxel));
        }
    }

    IEnumerator GrowVoxel(GameObject voxel)
    {
        bool grow = true;
        while (grow)
        {
            Vector3 normalSize = Vector3.one;
            voxel.SetActive(true);
            if (voxel.transform.localScale != normalSize)
            {
                voxel.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                voxel.transform.localScale = normalSize;
                grow = false;
            }

            yield return new WaitForSeconds(0.02f);
        }

        _coroutineFinished = true;
    }

    private IEnumerator ShrinkVoxel(GameObject voxel)
    {
        _coroutineFinished = false;
        bool shrink = true;
        while (shrink)
        {
            Vector3 smallSize = new Vector3(0.1f, 0.1f, 0.1f);
            if (voxel.transform.localScale != smallSize)
            {
                voxel.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                voxel.transform.localScale = smallSize;
                voxel.SetActive(false);
                shrink = false;
            }

            yield return new WaitForSeconds(0.02f);
        }

        _coroutineFinished = true;
    }

    private void ManageRotations()
    {
        Vector2 controllerRotation = SteamVR_Actions.picross.Rotate[SteamVR_Input_Sources.Any].axis;

        float horizontalMovement = SteamVR_Actions.picross.Rotate[SteamVR_Input_Sources.Any].axis.x;
        float verticalMovement = SteamVR_Actions.picross.Rotate[SteamVR_Input_Sources.Any].axis.y;

        bool rotateRight = false;
        bool rotateLeft = false;
        bool rotateUp = false;
        bool rotateDown = false;

        if (horizontalMovement >= .65f)
        {
            rotateRight = true;
        }
        if (horizontalMovement <= -.65f)
        {
            rotateLeft = true;
        }
        if (verticalMovement >= .65f)
        {
            rotateUp = true;
        }
        if (verticalMovement <= -.65f)
        {
            rotateDown = true;
        }


        float timeSpeed = rotateSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.J) || rotateLeft)
        {
            transform.RotateAround(_target, transform.up, timeSpeed);
        }

        if (Input.GetKey(KeyCode.L) || rotateRight)
        {
            transform.RotateAround(_target, transform.up, -timeSpeed);
        }

        if (Input.GetKey(KeyCode.I) || rotateUp && _canVerticallyRotate)
        {
            transform.RotateAround(_target, _cameraTransform.right, timeSpeed);
        }

        if (Input.GetKey(KeyCode.K) || rotateDown && _canVerticallyRotate)
        {
            transform.RotateAround(_target, _cameraTransform.right, -timeSpeed);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
        }
    }
    
    private VoxelInfo[,,] SetPuzzleVoxels(UInt32[] palette, byte[] flatData, Int32 sizeX, Int32 sizeY, Int32 sizeZ)
    {
        VoxelInfo[,,] solution = new VoxelInfo[sizeX, sizeY, sizeZ];
        Color[] colorPalette = new Color[palette.Length];

        // Parse colors
        byte a, r, g, b;
        for (int i = 0; i < palette.Length; i++)
        {
            palette[i].ToARGB(out a, out r, out g, out b);
            colorPalette[i] = new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }
        
        // Convert 1D array to 3D
        byte[,,] data = new byte[sizeX, sizeY, sizeZ];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                for (int k = 0; k < sizeZ; k++)
                {
                    data[i, j, k] = flatData[k + j * sizeZ + i * sizeY * sizeZ];
                }
            }
        }

        // Parse puzzle structure
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                for (int k = 0; k < sizeZ; k++)
                {
                    bool partOfSolution = data[i, j, k] != 0;

                    solution[i, j, k] = new VoxelInfo(
                        partOfSolution ? VoxelState.Marked : VoxelState.Cleared,
                        colorPalette[data[i, j, k]],
                        new Vector3Int(i, j, k));
                }
            }
        }

        return solution;
    }
    
    private VoxelState[,,] GetSolutionVoxelStates(VoxelInfo[,,] voxelInfoSolution)
    {
        int sizeX = voxelInfoSolution.GetLength(0);
        int sizeY = voxelInfoSolution.GetLength(1);
        int sizeZ = voxelInfoSolution.GetLength(2);

        VoxelState[,,] _states = new VoxelState[sizeX, sizeY, sizeZ];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                for (int k = 0; k < sizeZ; k++)
                {
                    _states[i, j, k] = voxelInfoSolution[i, j, k].VoxelState;
                }
            }
        }

        return _states;
    }
}