using UnityEngine;

public class ProjectileMovementContext
{
    public ProjectileInstance Projectile;

    public Rigidbody Rigidbody;

    public Projectile Template;

    public UnitBase Owner;

    public float Age;

    // Direction the projectile was fired.
    // Never changes.
    public Vector3 LaunchDirection { get; set; }

    // Current movement direction.
    // Updates automatically from velocity.
    public Vector3 VelocityDirection
    {
        get
        {
            Vector3 velocity = Rigidbody.linearVelocity;
            velocity.y = 0f;

            if (velocity.sqrMagnitude > 0.0001f)
                return velocity.normalized;

            return LaunchDirection;
        }
    }

    public Vector3 LaunchRight =>
        Vector3.Cross(Vector3.up, LaunchDirection);

    public Vector3 VelocityRight =>
        Vector3.Cross(Vector3.up, VelocityDirection);
}