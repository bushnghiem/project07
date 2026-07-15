using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Movement/Curve")]
public class CurveMovement : ProjectileMovement
{
    public float curveStrength = 5f;

    public override MovementContribution GetContribution(
        ProjectileMovementContext context)
    {
        return new MovementContribution
        {
            Reference = MovementReference.Launch,

            LateralAcceleration = curveStrength
        };
    }
}