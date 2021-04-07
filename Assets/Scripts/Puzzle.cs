using System;
using UnityEngine;
using CsharpVoxReader;

public class Puzzle : MonoBehaviour
{
    private VoxelInfo[,,] _solution;

    public void SetPuzzleVoxels(UInt32[] palette, byte[,,] data, Int32 sizeX, Int32 sizeY, Int32 sizeZ)
    {
        _solution = new VoxelInfo[sizeX, sizeY, sizeZ];
        Color[] colorPalette = new Color[palette.Length];

        // Parse colors
        byte a, r, g, b;
        for (int i = 0; i < palette.Length; i++)
        {
            palette[i].ToARGB(out a, out r, out g, out b);
            colorPalette[i] = new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }
        
        // Parse puzzle structure
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                for (int k = 0; k < sizeZ; k++)
                {
                    bool partOfSolution = data[i, j, k] != 0;
                    
                    _solution[i, j, k] = new VoxelInfo(
                        partOfSolution ? VoxelManager.VoxelState.Marked : VoxelManager.VoxelState.Cleared,
                        colorPalette[data[i, j, k]],
                        new Vector3Int(i, j, k));
                }
            }
        }
    }
}
