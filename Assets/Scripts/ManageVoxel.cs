using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageVoxel : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private bool _isHovering = false;
    private bool _cleared = false;
    private bool _marked = false;
    
    public Material defaultColor;
    public Material hoverColor;
    public Material markedColor;
    public Material clearColor;
    public bool isPuzzleVoxel;

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
            } else
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
            } else
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
        } else if (_marked)
        {
            _meshRenderer.material = markedColor;
        } else
        {
            _meshRenderer.material = defaultColor;
        }
    }
}
