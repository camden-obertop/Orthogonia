using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnVoxels : MonoBehaviour
{
    [SerializeField] private int length, height, width; // length = x, height = y, width = z 
    private int visibleLayersX, visibleLayersY, visibleLayersZ;
    [SerializeField] private float rotateSpeed;

    public GameObject mainCamera;
    public GameObject cube;

    private GameObject[,,] voxels;
    private Vector3 _target = Vector3.zero;
    private Transform cameraTransform;

    private void Start()
    {
        visibleLayersX = length - 1;
        visibleLayersY = height - 1;
        visibleLayersZ = width - 1;
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
        ManageVisibleLayers();
    }

    private void ManageVisibleLayers()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && visibleLayersX > 0)
        {
            for (int i = 0; i < visibleLayersY + 1; i++)
            {
                for (int j = 0; j < visibleLayersZ + 1; j++)
                {
                    voxels[visibleLayersX, i, j].SetActive(false);
                }
            }
            visibleLayersX--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && visibleLayersX < length - 1)
        {
            visibleLayersX++;
            for (int i = 0; i < visibleLayersY + 1; i++)
            {
                for (int j = 0; j < visibleLayersZ + 1; j++)
                {
                    voxels[visibleLayersX, i, j].SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && visibleLayersY > 0)
        {
            for (int i = 0; i < visibleLayersX + 1; i++)
            {
                for (int j = 0; j < visibleLayersZ + 1; j++)
                {
                    voxels[i, visibleLayersY, j].SetActive(false);
                }
            }
            visibleLayersY--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && visibleLayersY < height - 1)
        {
            visibleLayersY++;
            for (int i = 0; i < visibleLayersX + 1; i++)
            {
                for (int j = 0; j < visibleLayersZ + 1; j++)
                {
                    voxels[i, visibleLayersY, j].SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && visibleLayersZ > 0)
        {
            for (int i = 0; i < visibleLayersX + 1; i++)
            {
                for (int j = 0; j < visibleLayersY + 1; j++)
                {
                    voxels[i, j, visibleLayersZ].SetActive(false);
                }
            }
            visibleLayersZ--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && visibleLayersZ < width - 1)
        {
            visibleLayersZ++;
            for (int i = 0; i < visibleLayersX + 1; i++)
            {
                for (int j = 0; j < visibleLayersY + 1; j++)
                {
                    voxels[i, j, visibleLayersZ].SetActive(true);
                }
            }
        }
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
