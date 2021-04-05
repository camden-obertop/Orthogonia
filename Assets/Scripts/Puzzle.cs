using System;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private VoxelInfo[,,] _voxels;

    public void SetPuzzleVoxels(UInt32[] palette, byte[,,] data, Int32 sizeX, Int32 sizeY, Int32 sizeZ)
    {
        // You can use extension method ToARGB from namespace CsharpVoxReader
        // To get rgba values
        // ie: palette[1].ToARGB(out a, out r, out g, out b)
    }
}
