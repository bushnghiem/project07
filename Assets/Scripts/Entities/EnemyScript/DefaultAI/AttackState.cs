using UnityEngine;

public class AttackState : EnemyState
{
    public override UnitAction DecideAction(Enemy enemy, EnemyAIBase ai)
    {
        var typedAI = ai as StateMachineAI;
        if (typedAI == null) return null;

        var target = EnemyAIUtility.GetClosestPlayer(enemy, typedAI.battleManager);
        if (target == null) return null;

        Vector3 direction = target.Position - enemy.Position;
        direction.y = 0;
        direction.Normalize();

        float distance = Vector3.Distance(enemy.Position, target.Position);
        float maxRange = EnemyAIUtility.EstimateShotRange(enemy);

        float power = Mathf.Clamp01(distance / maxRange);

        float error = typedAI.aimErrorAngle * (distance / maxRange);
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