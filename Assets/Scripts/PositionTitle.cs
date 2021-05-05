using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTitle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _mainCamera;

    private Vector3 _initialPosition;

    void Start()
    {
        _initialPosition = transform.position;
    }


    void Update()
    {
        if (_mainCamera != null)
        {
            float height = _mainCamera.transform.position.y;
            transform.position = _initialPosition + new Vector3(0, height + .8f, 0);
        }
    }
}
