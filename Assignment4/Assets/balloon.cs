using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField]
    private float upwardForce = 50f;
    [SerializeField]
    private float rotationForce = 5f;

    private bool isHit = false;
    private Rigidbody balloonRigidbody;

    void Start()
    {
        balloonRigidbody = GetComponent<Rigidbody>();
        balloonRigidbody.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHit && collision.relativeVelocity.magnitude > 1)
        {
            HandleHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHit)
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        isHit = true;

        balloonRigidbody.useGravity = true;

        balloonRigidbody.velocity = Vector3.zero;

        // Apply upward force to make the balloon fly
        balloonRigidbody.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

        // Apply rotation force for a spinning effect
        balloonRigidbody.AddTorque(Vector3.up * rotationForce, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }
}
