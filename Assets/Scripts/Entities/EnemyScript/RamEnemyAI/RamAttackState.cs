using UnityEngine;

public class RamAttackState : EnemyState
{
    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);

        if (target == null)
            return null;

        Vector3 dir = target.Position - enemy.Position;

        dir.y = 0;
        dir.Normalize();

        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Move,
            direction = dir,
            powerPercent = 1f
        };
    }
}