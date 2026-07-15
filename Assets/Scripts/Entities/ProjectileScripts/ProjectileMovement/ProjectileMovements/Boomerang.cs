using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Movement/Boomerang")]
public class BoomerangMovement : ProjectileMovement
{
    [Header("Timing")]
    public float returnAfter = 0.75f;

    [Header("Movement")]
    public float forwardAcceleration = 10f;
    public float curveStrength = 12f;
    public float returnSteering = 20f;

    public override MovementContribution GetContribution(
        ProjectileMovementContext context)
    {
        MovementContribution contribution = new MovementContribution
        {
            Reference = MovementReference.Launch,

            ForwardAcceleration = forwardAcceleration,

            LateralAcceleration = curveStrength
        };

        // Don't return yet
        if (context.Age < returnAfter)
            return contribution;

        // Begin steering toward the owner
        if (context.Owner != null)
        {
            Vector3 toOwner =
                context.Owner.Position -
                context.Projectile.Position;

            toOwner.y = 0f;

            if (toOwner.sqrMagnitude > 0.01f)
            {
                contribution.WorldAcceleration =
                    toOwner.normalized * returnSteering;
            }
        }

        return contribution;
    }
}