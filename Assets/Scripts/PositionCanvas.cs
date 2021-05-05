using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCanvas : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject _block;

    void Update()
    {
        transform.position = _block.transform.position + new Vector3(0, .125f, 0);
    }
}
