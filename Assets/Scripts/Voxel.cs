using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("Materials")]
    [SerializeField] private Material defaultColor;
    [SerializeField] private Material hoverColor;
    [SerializeField] private Material markedColor;
    [SerializeField] private Material clearColor;

    [Header("Texts")]
    [SerializeField] private HintText frontHint;
    [SerializeField] private HintText rightSideHint;
    [SerializeField] private HintText topHint;
    [SerializeField] private HintText rearHint;
    [SerializeField] private HintText leftSideHint;
    [SerializeField] private HintText bottomHint;

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

    private HintText[] hints = new HintText[6];

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        hints[0] = frontHint;
        hints[1] = rightSideHint;
        hints[2] = topHint;
        hints[3] = rearHint;
        hints[4] = leftSideHint;
        hints[5] = bottomHint;
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

    public void SetSideText(VoxelSide side, int voxelCount, int gapCount)
    {
        hints[(int)side].SetHintText(voxelCount, gapCount);
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
