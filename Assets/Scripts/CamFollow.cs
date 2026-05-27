using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Entity target;

    private Unit currentUnit;

    public Vector3 offset;
    public float smoothTime = 0.2f;

    private Vector3 velocity;

    private void OnEnable()
    {
        TurnEvent.OnUnitTurnStart += HandleUnitTurnStart;
        DeathEvent.OnEntityDeath += HandleDeath;
        ProjectileSpawnEvent.AddCamFollow += HandleProjectileFollow;
    }

    private void OnDisable()
    {
        TurnEvent.OnUnitTurnStart -= HandleUnitTurnStart;
        DeathEvent.OnEntityDeath -= HandleDeath;
        ProjectileSpawnEvent.AddCamFollow -= HandleProjectileFollow;
    }

    void HandleUnitTurnStart(Unit unit)
    {
        currentUnit = unit;

        // Don't override projectile tracking if desired
        if (!(target is Projectile))
        {
            target = unit;
        }
    }

    void HandleProjectileFollow(Entity entity)
    {
        target = entity;
    }

    void HandleDeath(Entity entity)
    {
        if (entity == target)
        {
            target = currentUnit;
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.Position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}