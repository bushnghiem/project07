using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EntitySpeedLimiter : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 20f;

    private Rigidbody rb;

    public float MaxSpeed => maxSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Detect fast-moving objects more reliably.
        rb.collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;
    }

    private void FixedUpdate()
    {
        LimitVelocity();
    }

    public void SetMaxSpeed(float speed)
    {
        maxSpeed = Mathf.Max(0f, speed);

        LimitVelocity();
    }

    private void LimitVelocity()
    {
        if (rb == null)
            return;

        Vector3 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.linearVelocity = velocity.normalized * maxSpeed;
        }
    }
}
