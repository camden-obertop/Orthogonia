using UnityEngine;

public class Voxel : MonoBehaviour
{
    [SerializeField] private Material defaultColor;
    [SerializeField] private Material hoverColor;
    [SerializeField] private Material markedColor;
    [SerializeField] private Material clearColor;

    private bool _isPuzzleVoxel;
    public bool IsPuzzleVoxel
    {
        get => _isPuzzleVoxel;
        set => _isPuzzleVoxel = value;
    }

    private MeshRenderer _meshRenderer;
    private bool _isHovering = false;
    private bool _cleared = false;
    private bool _marked = false;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClearVoxel();
        }

        if (Input.GetMouseButtonDown(1))
        {
            MarkVoxel();
        }
    }

    private void MarkVoxel()
    {
        if (_isHovering && !_cleared)
        {
            if (_marked)
            {
                _marked = false;
                _meshRenderer.material = hoverColor;
            }
            else
            {
                _marked = true;
                _meshRenderer.material = markedColor;
            }
        }
    }

    private void ClearVoxel()
    {
        if (_isHovering)
        {
            if (_cleared)
            {
                _cleared = false;
                _meshRenderer.material = hoverColor;
            }
            else
            {
                _cleared = true;
                _marked = false;
                _meshRenderer.material = clearColor;
            }
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
        if (_cleared)
        {
            _meshRenderer.material = clearColor;
        }
        else if (_marked)
        {
            _meshRenderer.material = markedColor;
        }
        else
        {
            _meshRenderer.material = defaultColor;
        }
    }
}
