using UnityEngine;

public class Voxel : MonoBehaviour {
    [SerializeField] private Material defaultColor;
    [SerializeField] private Material hoverColor;
    [SerializeField] private Material markedColor;
    [SerializeField] private Material clearColor;

    private bool _isPuzzleVoxel;
    public bool IsPuzzleVoxel {
        get => _isPuzzleVoxel;
        set => _isPuzzleVoxel = value;
    }

    private bool _isVisible = true;
    public bool IsVisible {
        get => _isVisible;
        set => _isVisible = value;
    }

    private bool _isMarked = false;
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
    
    private bool _isHovering = false;
    public bool IsHovering
    {
        get => _isHovering;
        set => _isHovering = value;
    }

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (_isHovering && Input.GetMouseButtonDown(0))
        {
            if (_manager.CurrentGameMode == VoxelManager.GameMode.Build) {
                BuildVoxel();
            }

            if (_manager.CurrentGameMode == VoxelManager.GameMode.Destroy) {
                ClearVoxel();
            }

            if (_manager.CurrentGameMode == VoxelManager.GameMode.Mark) {
                MarkVoxel();
            }
        }
    }

    private void BuildVoxel() {
        if (!_isVisible) {
            _isVisible = true;
            _meshRenderer.material = hoverColor;
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
        }
    }

    private void OnMouseEnter()
    {
        _isHovering = true;
        _meshRenderer.material = hoverColor;
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
}
