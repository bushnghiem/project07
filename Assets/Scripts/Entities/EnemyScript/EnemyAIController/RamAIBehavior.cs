using UnityEngine;

[CreateAssetMenu(menuName = "AI/Ram AI")]
public class RamAIBehavior : EnemyAIBehavior
{
    public float ramDistanceThreshold = 3f;

    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context)
    {
        Player target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);

        if (target == null)
            return null;

        float distance = Vector3.Distance(enemy.Position, target.Position);

        Vector3 dir = (target.Position - enemy.Position).normalized;

        if (distance <= ramDistanceThreshold)
        {
            return new UnitAction
            {
                actor = enemy,
                actionType = ActionType.Move,
                direction = dir,
                powerPercent = 1f
            };
        }

        dir = EnemyAIUtility.GetSteeredDirection(enemy, dir);

        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Move,
            direction = dir,
            powerPercent = 1f
        };
    }
}