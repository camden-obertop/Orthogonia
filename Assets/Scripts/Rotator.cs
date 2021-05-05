using UnityEngine;
using Random = UnityEngine.Random;

public class Rotator : MonoBehaviour
{
    [Header("Variables")] 
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _randomizeOrientation;

    private void Start()
    {
        if (_randomizeOrientation)
        {
            transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, _rotateSpeed, 0));
    }
}