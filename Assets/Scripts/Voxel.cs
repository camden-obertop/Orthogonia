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

    [SerializeField] private bool _isPuzzleVoxel;
    public bool IsPuzzleVoxel
    {
        get => _isPuzzleVoxel;
        set => _isPuzzleVoxel = value;
    }

    private MeshRenderer _meshRenderer;
    private bool _isHovering = false;
    private bool _cleared = false;
    private bool _marked = false;

    private HintText[] _hints = new HintText[6];
    private bool _hintArraySet;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        SetHintArray();
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
        SetHintArray();
        _hints[(int)side].SetHintText(voxelCount, gapCount);
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
