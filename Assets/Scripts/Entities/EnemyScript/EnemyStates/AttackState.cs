using UnityEngine;

public class AttackState : EnemyState
{
    public override UnitAction DecideAction(
        Enemy enemy,
        StateMachineAI ai)
    {
        var target =
            EnemyAIUtility.GetClosestPlayer(
                enemy,
                ai.battleManager
            );

        if (target == null)
            return null;

        Vector3 direction =
            target.Position -
            enemy.Position;

        direction.y = 0;
        direction.Normalize();

        float distance =
            Vector3.Distance(
                enemy.Position,
                target.Position
            );

        float maxRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        float power =
            Mathf.Clamp01(
                distance / maxRange
            );

        float error =
            ai.aimErrorAngle *
            (distance / maxRange);

        direction =
            EnemyAIUtility.ApplyAimError(
                direction,
                error
            );

        return new UnitAction
        {
            actor = enemy,

            actionType =
                ActionType.Shoot,

            direction = direction,

            powerPercent = power,

            projectile =
                enemy.GetProjectile()
        };
    }
}