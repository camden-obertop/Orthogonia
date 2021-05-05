using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] private float _rotateSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0, _rotateSpeed, 0));
    }
}
