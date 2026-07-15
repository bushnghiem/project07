using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Movement/Constant Acceleration")]
public class ConstantAccelerationMovement : ProjectileMovement
{
    public float acceleration = 10f;

    public override MovementContribution GetContribution(
        ProjectileMovementContext context)
    {
        return new MovementContribution
        {
            Reference = MovementReference.Launch,

            ForwardAcceleration = acceleration
        };
    }
}