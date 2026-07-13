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
        FaceActionDirection(action);

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
        FaceActionDirection(action);

        AttackContext attack = new AttackContext();

        CameraEvent.FollowTarget?.Invoke(
            attack.CameraTarget);

        CameraEvent.LockCamera?.Invoke();

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

        Projectile projectile =
            action.projectile;

        ShotPattern pattern =
            ShotPatternFactory.CreateBasicShot(
                action.actor,
                projectile,
                action.direction,
                force
            );

        pattern.attackContext = attack;

        foreach (var modifier in action.actor.ShotModifiers)
        {
            modifier.Modify(
                pattern,
                action.actor
            );
        }

        ShotPatternSpawner.Spawn(
            pattern,
            action.actor
        );

        action.actor.TriggerShootEffects(action.direction, force);

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
                action.itemTargetData
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

    private void FaceActionDirection(UnitAction action)
    {
        Vector3 dir = action.direction;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        action.actor.transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, 180f, 0f);
    }
}