using System.Collections;
using UnityEngine;

public class GroundWander : MonoBehaviour
{
    [SerializeField] private float wanderTime;
    [SerializeField] private float pauseTime;
    [SerializeField] private float wanderSpeed;

    private enum GroundWanderState
    {
        PAUSED,
        ROTATING,
        WANDERING
    }

    private GroundWanderState _currentState = GroundWanderState.PAUSED;
    private Coroutine _wanderingWait;
    private Coroutine _pauseWait;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // update current state
        switch (_currentState)
        {
            case GroundWanderState.PAUSED:
                _rigidbody.velocity = Vector3.zero;
                break;
            case GroundWanderState.ROTATING:
                _rigidbody.velocity = Vector3.zero;
                transform.rotation *= Quaternion.Euler(0f, Random.Range(160f, 200f), 0f);
                break;
            case GroundWanderState.WANDERING:
                _rigidbody.velocity = transform.forward * wanderSpeed;
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
            case GroundWanderState.ROTATING:
                _currentState = GroundWanderState.WANDERING;
                break;
            case GroundWanderState.WANDERING:
                if (_wanderingWait == null)
                {
                    _wanderingWait = StartCoroutine(WaitForWander());
                }
                break;
        }
    }

    public void CollideWithGround()
    {
        if (_currentState == GroundWanderState.WANDERING)
        {
            _currentState = GroundWanderState.ROTATING;
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
        _pauseWait = null;
    }
}
