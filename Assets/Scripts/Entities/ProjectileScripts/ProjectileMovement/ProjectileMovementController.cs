using UnityEngine;
using System.Collections.Generic;

public class ProjectileMovementController : MonoBehaviour
{
    private readonly List<ProjectileMovement> runtimeMovements = new();

    private ProjectileMovementContext context;

    private Rigidbody rb;

    public void Initialize(
        Projectile projectile,
        ProjectileInstance instance,
        UnitBase owner,
        Vector3 launchDirection)
    {
        rb = GetComponent<Rigidbody>();

        context = new ProjectileMovementContext
        {
            Projectile = instance,
            Rigidbody = rb,
            Template = projectile,
            Owner = owner,
            LaunchDirection = launchDirection.normalized
        };

        runtimeMovements.Clear();

        foreach (var movement in projectile.movements)
        {
            if (movement == null)
                continue;

            ProjectileMovement runtime =
                Instantiate(movement);

            runtime.Initialize(context);

            runtimeMovements.Add(runtime);
        }
    }

    private void FixedUpdate()
    {
        if (context == null)
            return;

        context.Age += Time.fixedDeltaTime;

        Vector3 totalAcceleration = Vector3.zero;

        foreach (var movement in runtimeMovements)
        {
            movement.UpdateContext(
                context,
                Time.fixedDeltaTime);

            MovementContribution contribution =
                movement.GetContribution(context);

            Vector3 forward;
            Vector3 right;

            if (contribution.Reference == MovementReference.Launch)
            {
                forward = context.LaunchDirection;
                right = context.LaunchRight;
            }
            else
            {
                forward = context.VelocityDirection;
                right = context.VelocityRight;
            }

            totalAcceleration +=
                forward * contribution.ForwardAcceleration;

            totalAcceleration +=
                right * contribution.LateralAcceleration;

            totalAcceleration +=
                contribution.WorldAcceleration;
        }

        rb.AddForce(
            totalAcceleration,
            ForceMode.Acceleration);
    }
}