using UnityEngine;

[CreateAssetMenu(menuName = "AI/Ram AI")]
public class RamAIBehavior : EnemyAIBehavior
{
    [Header("Ram Behavior")]
    [Tooltip("Within this distance, the enemy commits to a direct ram.")]
    public float directRamDistance = 3f;

    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context)
    {
        Player target =
            EnemyAIUtility.GetClosestPlayer(
                enemy,
                battleManager);

        if (target == null)
            return null;

        // Get horizontal direction to target.
        Vector3 offset =
            target.Position -
            enemy.Position;

        offset.y = 0f;

        float distance = offset.magnitude;

        // Already essentially on top of the target.
        if (distance < 0.001f)
            return null;

        Vector3 direction =
            offset.normalized;

        // At longer range, use steering so the rammer
        // can navigate around obstacles.
        if (distance > directRamDistance)
        {
            direction =
                EnemyAIUtility.GetSteeredDirection(
                    enemy,
                    direction);
        }

        // A rammer always uses maximum movement power.
        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Move,
            direction = direction,
            powerPercent = 1f
        };
    }
}