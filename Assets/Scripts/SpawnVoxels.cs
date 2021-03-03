using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnVoxels : MonoBehaviour
{
    [SerializeField] private int length, height, width; // length = x, height = y, width = z 

    public GameObject cube;

    private GameObject[,,] voxels;

    private void Start()
    {
        InitializeVoxels();
        DeactivateFace();
    }

    private void InitializeVoxels()
    {
        voxels = new GameObject[length, height, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    voxels[i, j, k] = Instantiate(cube, new Vector3(i + 0.5f, j + 0.5f, k + 0.5f), Quaternion.identity, transform);
                    voxels[i, j, k].GetComponent<ManageVoxel>().isPuzzleVoxel = Convert.ToBoolean(Random.Range(0, 2));
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Deactivate a layer of the length
            
        } 
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Reactivate a layer of the length
        }
    }

    private void DeactivateFace()
    {
        foreach(GameObject voxel in voxels)
        {
            Debug.Log(voxel);
        }
    }
}
