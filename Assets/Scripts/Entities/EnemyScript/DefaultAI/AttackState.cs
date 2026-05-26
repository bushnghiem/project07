using UnityEngine;

public class AttackState : EnemyState
{
    private float aimErrorAngle;

    public AttackState(float aimErrorAngle)
    {
        this.aimErrorAngle = aimErrorAngle;
    }

    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);

        if (target == null)
            return null;

        Vector3 direction = target.Position - enemy.Position;

        direction.y = 0;
        direction.Normalize();

        float distance = Vector3.Distance(enemy.Position, target.Position);

        float maxRange = EnemyAIUtility.EstimateShotRange(enemy);

        float power = Mathf.Clamp01(distance / maxRange);

        float error = aimErrorAngle * (distance / maxRange);

        direction = EnemyAIUtility.ApplyAimError(direction, error);

        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Shoot,
            direction = direction,
            powerPercent = power,
            projectile = enemy.GetProjectile()
        };
    }
}