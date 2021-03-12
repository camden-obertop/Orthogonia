using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class VoxelManager : MonoBehaviour
{
    [SerializeField] private int length, height, width; // length = x, height = y, width = z 
    [SerializeField] private float rotateSpeed;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject cube;

    private int visibleLayersX, visibleLayersY, visibleLayersZ;
    private GameObject[,,] voxels;
    private Vector3 _target = Vector3.zero;
    private Transform cameraTransform;
    private bool coroutineFinished = true;
    private bool canVerticallyRotate = true;

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
                    voxels[i, j, k] = Instantiate(cube, new Vector3(i - length / 2, j - height / 2, k - width / 2), Quaternion.identity, transform);
                    voxels[i, j, k].GetComponent<Voxel>().IsPuzzleVoxel = Convert.ToBoolean(Random.Range(0, 2));
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
        if (coroutineFinished)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && visibleLayersX > 0)
            {
                for (int i = 0; i < visibleLayersY + 1; i++)
                {
                    for (int j = 0; j < visibleLayersZ + 1; j++)
                    {
                        ChangeVoxelVisible(voxels[visibleLayersX, i, j], false);
                        // voxels[visibleLayersX, i, j].SetActive(false);
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
                        ChangeVoxelVisible(voxels[visibleLayersX, i, j], true);
                        // voxels[visibleLayersX, i, j].SetActive(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && visibleLayersY > 0)
            {
                for (int i = 0; i < visibleLayersX + 1; i++)
                {
                    for (int j = 0; j < visibleLayersZ + 1; j++)
                    {
                        ChangeVoxelVisible(voxels[i, visibleLayersY, j], false);
                        // voxels[i, visibleLayersY, j].SetActive(false);
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
                        ChangeVoxelVisible(voxels[i, visibleLayersY, j], true);
                        // voxels[i, visibleLayersY, j].SetActive(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && visibleLayersZ > 0)
            {
                for (int i = 0; i < visibleLayersX + 1; i++)
                {
                    for (int j = 0; j < visibleLayersY + 1; j++)
                    {
                        ChangeVoxelVisible(voxels[i, j, visibleLayersZ], false);
                        // voxels[i, j, visibleLayersZ].SetActive(false);
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
                        ChangeVoxelVisible(voxels[i, j, visibleLayersZ], true);
                        // voxels[i, j, visibleLayersZ].SetActive(true);
                    }
                }
            }
        }
    }

    private void ChangeVoxelVisible(GameObject voxel, bool isActivated)
    {
        if (isActivated)
        {
            coroutineFinished = false;
            StartCoroutine(GrowVoxel(voxel));
        }
        else
        {
            coroutineFinished = false;
            StartCoroutine(ShrinkVoxel(voxel));
        }
    }

    IEnumerator GrowVoxel(GameObject voxel)
    {
        bool grow = true;
        while (grow)
        {
            Vector3 normalSize = Vector3.one;
            voxel.SetActive(true);
            if (voxel.transform.localScale != normalSize)
            {
                voxel.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                voxel.transform.localScale = normalSize;
                grow = false;
            }
            yield return new WaitForSeconds(0.02f);
        }
        coroutineFinished = true;
    }

    private IEnumerator ShrinkVoxel(GameObject voxel)
    {
        coroutineFinished = false;
        bool shrink = true;
        while (shrink)
        {
            Vector3 smallSize = new Vector3(0.1f, 0.1f, 0.1f);
            if (voxel.transform.localScale != smallSize)
            {
                voxel.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                voxel.transform.localScale = smallSize;
                voxel.SetActive(false);
                shrink = false;
            }
            yield return new WaitForSeconds(0.02f);
        }
        coroutineFinished = true;
    }

    private void ManageRotations()
    {
        float timeSpeed = rotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(_target, transform.up, timeSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(_target, transform.up, -timeSpeed);
        }

        if (Input.GetKey(KeyCode.W) && canVerticallyRotate)
        {
            transform.RotateAround(_target, cameraTransform.right, timeSpeed);
        }

        if (Input.GetKey(KeyCode.S) && canVerticallyRotate)
        {
            transform.RotateAround(_target, cameraTransform.right, -timeSpeed);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
