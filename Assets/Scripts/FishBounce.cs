using System.Collections;
using UnityEngine;

public class FishBounce : MonoBehaviour
{
    private float _initY;
    private float _offset;

    private void Start()
    {
        _initY = transform.position.y;
        _offset = Random.Range(0, 100f);
    }

    private void Update()
    {
        Vector3 newPos = transform.position;
        float newY = Mathf.Sin(Time.time * 2 + _offset) + _initY;
        transform.position = new Vector3(newPos.x, newY, newPos.z);
    }
}