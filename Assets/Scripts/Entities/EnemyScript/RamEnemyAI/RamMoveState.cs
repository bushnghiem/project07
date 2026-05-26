using UnityEngine;

public class RamMoveState : EnemyState
{
    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);

        if (target == null)
            return null;

        Vector3 dir =
            target.Position - enemy.Position;

        dir.y = 0;

        dir.Normalize();

        dir = EnemyAIUtility.GetSteeredDirection(enemy, dir);

        float distance = Vector3.Distance(enemy.Position, target.Position);

        float maxMove = EnemyAIUtility.EstimateMoveRange(enemy);

        float power = Mathf.Lerp(0.6f, 1f, distance / maxMove);

        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Move,
            direction = dir,
            powerPercent = power
        };
    }
}