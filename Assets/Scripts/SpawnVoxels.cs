using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnVoxels : MonoBehaviour
{
    [SerializeField] private int length, height, width; // length = x, height = y, width = z 

    public GameObject cube;

    private void Start()
    {
        InitializeVoxels();
    }

    private void InitializeVoxels()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    GameObject voxel = Instantiate(cube, new Vector3(i + 0.5f, j + 0.5f, k + 0.5f), Quaternion.identity, transform);
                    voxel.GetComponent<ManageVoxel>().isPuzzleVoxel = Convert.ToBoolean(Random.Range(0, 2));
                }
            }
        }
    }
}
