using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVoxels : MonoBehaviour
{
    [SerializeField] private int length, width, height; // length = x, width = z, height = y

    public GameObject cube;

    private void Start()
    {
        InitializeVoxels();
    }

    private void InitializeVoxels()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    int random = Random.Range(0, 2);
                    if (random == 1)
                    {
                        Instantiate(cube, new Vector3(i + 0.5f, k + 0.5f, j + 0.5f), Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
