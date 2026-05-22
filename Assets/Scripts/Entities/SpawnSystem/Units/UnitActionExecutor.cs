using UnityEngine;

public class UnitActionExecutor : MonoBehaviour
{
    public float projectileSpawnRadius = 2f;

    public void Execute(UnitAction action)
    {
        if (action == null || action.actor == null)
            return;

        switch (action.actionType)
        {
            case ActionType.Move:
                ExecuteMove(action);
                break;

            case ActionType.Shoot:
                ExecuteShoot(action);
                break;

            case ActionType.Item:
                ExecuteItem(action);
                break;
        }
    }

    private void ExecuteMove(UnitAction action)
    {
        Rigidbody rb =
            action.actor.GetComponent<Rigidbody>();

        float maxForce =
            action.actor.GetStat(
                ShipStatType.MoveStrength
            );

        float force =
            Mathf.Lerp(
                0f,
                maxForce,
                action.powerPercent
            );

        rb.AddForce(
            action.direction.normalized * force,
            ForceMode.Impulse
        );

        action.actor.Moved();

        ResolveAction(action);
    }

    private void ExecuteShoot(UnitAction action)
    {
        float maxForce =
            action.actor.GetStat(
                ShipStatType.ShotStrength
            );

        float force =
            Mathf.Lerp(
                0f,
                maxForce,
                action.powerPercent
            );

        bool handled =
            action.actor.TriggerShootEffects(
                action.direction,
                force
            );

        if (!handled)
        {
            Vector3 spawnPos =
                action.actor.Position +
                action.direction.normalized *
                projectileSpawnRadius;

            ProjectileSpawnEvent
                .OnProjectileSpawn?.Invoke(
                    spawnPos,
                    action.direction.normalized,
                    force,
                    action.projectile,
                    action.actor
                );
        }

        action.actor.Shot();

        ResolveAction(action);
    }

    private void ExecuteItem(UnitAction action)
    {
        if (action.activeItem == null)
            return;

        bool used =
            action.activeItem.Use(
                action.actor,
                action.actor
            );

        if (!used)
            return;

        ResolveAction(action);
    }

    private void ResolveAction(UnitAction action)
    {
        action.actor.SpendAP(action.apCost);

        action.actor.ActionResolved();
    }
}