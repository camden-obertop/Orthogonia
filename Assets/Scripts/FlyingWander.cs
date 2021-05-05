using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingWander : MonoBehaviour
{
    [SerializeField] private float wanderTime;
    [SerializeField] private float pauseTime;
    [SerializeField] private float wanderSpeed;

    private enum GroundWanderState
    {
        PAUSED,
        WANDERING
    }

    private GroundWanderState _currentState = GroundWanderState.PAUSED;
    private Coroutine _wanderingWait;
    private Coroutine _pauseWait;
    private bool _pauseInitialized;
    private float _initY;

    private void Start()
    {
        _initY = transform.position.y;
    }

    private void Update()
    {
        // update current state
        switch (_currentState)
        {
            case GroundWanderState.PAUSED:
                if (!_pauseInitialized)
                {
                    transform.rotation = Quaternion.Euler(0f, transform.rotation.y + Random.Range(0, 360), 0f);
                    _pauseInitialized = true;
                }

                Vector3 newPos = transform.position;
                float newY = Mathf.Sin(Time.time * 2) * 0.15f + _initY;
                transform.position = new Vector3(newPos.x, newY, newPos.z);
                break;
            case GroundWanderState.WANDERING:
                transform.position += transform.forward * wanderSpeed / 100f;
                break;
        }

        // check if need to transition to other states
        switch (_currentState)
        {
            case GroundWanderState.PAUSED:
                if (_pauseWait == null)
                {
                    _pauseWait = StartCoroutine(WaitForPause());
                }

                break;
            case GroundWanderState.WANDERING:
                if (_wanderingWait == null)
                {
                    _wanderingWait = StartCoroutine(WaitForWander());
                }

                break;
        }
    }

    private IEnumerator WaitForWander()
    {
        yield return new WaitForSeconds(wanderTime);
        _currentState = GroundWanderState.PAUSED;
        _wanderingWait = null;
    }

    private IEnumerator WaitForPause()
    {
        yield return new WaitForSeconds(pauseTime + Random.Range(0.0f, 0.9f));
        _currentState = GroundWanderState.WANDERING;
        transform.position = new Vector3(transform.position.x, _initY, transform.position.z);
        _pauseInitialized = false;
        _pauseWait = null;
    }
}