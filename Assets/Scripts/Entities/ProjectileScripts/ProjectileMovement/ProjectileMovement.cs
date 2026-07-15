using UnityEngine;

public abstract class ProjectileMovement : ScriptableObject
{
    public virtual void Initialize(ProjectileMovementContext context)
    {
    }

    public virtual void UpdateContext(
        ProjectileMovementContext context,
        float deltaTime)
    {
    }

    public abstract MovementContribution GetContribution(
        ProjectileMovementContext context);
}