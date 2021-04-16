using UnityEngine;
using Valve.VR;

public enum VoxelSide
{
    Front = 0,
    RightSide = 1,
    Top = 2,
    Rear = 3,
    LeftSide = 4,
    Bottom = 5
}

public class Voxel : MonoBehaviour
{
    [Header("Materials")] [SerializeField] private Material defaultColor;
    [SerializeField] private Material hoverColor;
    [SerializeField] private Material hoverDestroyColor;
    [SerializeField] private Material markedColor;
    [SerializeField] private Material clearColor;

    [Header("Texts")] [SerializeField] private HintText frontHint;
    [SerializeField] private HintText rightSideHint;
    [SerializeField] private HintText topHint;
    [SerializeField] private HintText rearHint;
    [SerializeField] private HintText leftSideHint;
    [SerializeField] private HintText bottomHint;

    [SerializeField] private bool _isPuzzleVoxel;

    public bool IsPuzzleVoxel
    {
        get => _isPuzzleVoxel;
        set => _isPuzzleVoxel = value;
    }

    private bool _isVisible = true;
    public bool IsVisible
    {
        get => _isVisible;
        set => _isVisible = value;
    }

    private bool _isMarked;
    public bool IsMarked
    {
        get => _isMarked;
        set => _isMarked = value;
    }

    private VoxelManager _manager;
    public VoxelManager Manager
    {
        get => _manager;
        set => _manager = value;
    }

    private bool _isHovering;
    public bool IsHovering
    {
        get => _isHovering;
        set => _isHovering = value;
    }

    private Vector3 _indexPosition;
    public Vector3 IndexPosition
    {
        get => _indexPosition;
        set => _indexPosition = value;
    }

    private HintText[] _hints = new HintText[6];
    public HintText[] Hints => _hints;
    
    private bool _hintArraySet;
    
    private MeshRenderer _meshRenderer;
    private bool performAction;



    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        SetHintArray();
    }

    private void Update()
    {
        performAction = SteamVR_Actions.picross.PerformAction[SteamVR_Input_Sources.Any].stateDown;
        if (_isHovering && (Input.GetMouseButtonDown(0) || performAction))
        {
            if (_manager.CurrentGameMode == VoxelManager.GameMode.Build)
            {
                BuildVoxel();
            }

            if (_manager.CurrentGameMode == VoxelManager.GameMode.Destroy)
            {
                ClearVoxel();
            }

            if (_manager.CurrentGameMode == VoxelManager.GameMode.Mark)
            {
                MarkVoxel();
            }
        }
    }

    private void BuildVoxel()
    {
        if (!_isVisible)
        {
            foreach (HintText hint in _hints)
            {
                hint.gameObject.SetActive(true);
            }
            
            _manager.UpdateAdjacentVoxelHints(_indexPosition);
            
            _isVisible = true;
            _meshRenderer.material = hoverColor;
        }
    }

    public void SetSideText(VoxelSide side, int voxelCount, int gapCount)
    {
        SetHintArray();
        _hints[(int) side].SetHintText(voxelCount, gapCount);
    }

    private void SetHintArray()
    {
        if (!_hintArraySet)
        {
            _hintArraySet = true;
            _hints[0] = frontHint;
            _hints[1] = rightSideHint;
            _hints[2] = topHint;
            _hints[3] = rearHint;
            _hints[4] = leftSideHint;
            _hints[5] = bottomHint;
        }
    }

    private void MarkVoxel()
    {
        if (_isVisible)
        {
            if (_isMarked)
            {
                _isMarked = false;
                _meshRenderer.material = hoverColor;
            }
            else
            {
                _isMarked = true;
                _meshRenderer.material = markedColor;
            }
        }
    }

    private void ClearVoxel()
    {
        foreach (HintText hint in _hints)
        {
            hint.gameObject.SetActive(!_isVisible);
        }
        
        if (!_isVisible)
        {
            _isVisible = true;
            _meshRenderer.material = hoverColor;
        }
        else
        {
            transform.gameObject.SetActive(false);
            _isVisible = false;
            _isMarked = false;
            _meshRenderer.material = clearColor;
            
            _manager.UpdateAdjacentVoxelHints(_indexPosition);
        }
    }

    private void OnMouseEnter()
    {
        _isHovering = true;
        if (_manager.CurrentGameMode == VoxelManager.GameMode.Destroy) {
            _meshRenderer.material = hoverDestroyColor;
        } else {
            _meshRenderer.material = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        _isHovering = false;
        if (!_isVisible)
        {
            _meshRenderer.material = clearColor;
        }
        else if (_isMarked)
        {
            _meshRenderer.material = markedColor;
        }
        else
        {
            _meshRenderer.material = defaultColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.CompareTag("Interactor"))
        {
            _isHovering = true;
            if (_manager.CurrentGameMode == VoxelManager.GameMode.Destroy) {
                _meshRenderer.material = hoverDestroyColor;
            } else {
                _meshRenderer.material = hoverColor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactor"))
        {
            _isHovering = false;
            if (!_isVisible)
            {
                _meshRenderer.material = clearColor;
            }
            else if (_isMarked)
            {
                _meshRenderer.material = markedColor;
            }
            else
            {
                _meshRenderer.material = defaultColor;
            }
        }
    }
}