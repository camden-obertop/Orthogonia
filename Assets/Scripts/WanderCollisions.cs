using UnityEngine;

public class WanderCollisions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            transform.parent.gameObject.GetComponent<GroundWander>().CollideWithGround();
        }
    }
}
