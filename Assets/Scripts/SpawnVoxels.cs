using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnVoxels : MonoBehaviour
{
    [SerializeField] private int length, height, width; // length = x, height = y, width = z 
    [SerializeField] private float rotateSpeed;

    public GameObject mainCamera;
    public GameObject cube;

    private GameObject[,,] voxels;
    private Vector3 _target = Vector3.zero;
    private Transform cameraTransform;

    private void Start()
    {
        InitializeVoxels();
        cameraTransform = mainCamera.transform;
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
                    voxels[i, j, k] = Instantiate(cube, new Vector3(i - length/2, j - height/2, k - width/2), Quaternion.identity, transform);
                    voxels[i, j, k].GetComponent<ManageVoxel>().isPuzzleVoxel = Convert.ToBoolean(Random.Range(0, 2));
                }
            }
        }
    }

    private void Update()
    {
        ManageRotations();
    }

    private void ManageRotations()
    {
        float timeSpeed = rotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(_target, Vector3.up, timeSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(_target, Vector3.up, -timeSpeed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.RotateAround(_target, cameraTransform.right, timeSpeed);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.RotateAround(_target, cameraTransform.right, -timeSpeed);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
