using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Movement/Zig Zag")]
public class ZigZagMovement : ProjectileMovement
{
    public float strength = 8f;

    public float frequency = 5f;

    public override MovementContribution GetContribution(
        ProjectileMovementContext context)
    {
        return new MovementContribution
        {
            Reference = MovementReference.Velocity,

            LateralAcceleration =
                Mathf.Sin(context.Age * frequency) * strength
        };
    }
}