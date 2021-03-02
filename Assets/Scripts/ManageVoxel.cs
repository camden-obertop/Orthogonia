using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageVoxel : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private bool _isHovering;
    
    public Material defaultColor;
    public Material hoverColor;
    public bool isPuzzleVoxel;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _isHovering = false;
    }

    private void OnMouseDown()
    {
        if (_isHovering)
        {
            if (isPuzzleVoxel)
            {
                Debug.Log("WRONG!!!");
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
    }

    private void OnMouseEnter()
    {
        _isHovering = true;
        _meshRenderer.material = hoverColor;
        Debug.Log(isPuzzleVoxel);
    }

    private void OnMouseExit()
    {
        _isHovering = false;
        _meshRenderer.material = defaultColor;
    }
}
