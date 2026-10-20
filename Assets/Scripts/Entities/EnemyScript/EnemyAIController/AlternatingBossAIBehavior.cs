using UnityEngine;

[CreateAssetMenu(menuName = "AI/Alternating Boss AI")]
public class AlternatingBossAIBehavior : EnemyAIBehavior
{
    [Header("AI Behaviors")]
    public ItemUsingAIBehavior normalBehavior;
    public RamAIBehavior ramBehavior;

    [Header("Starting Mode")]
    public bool startWithNormalTurn = true;

    // Called automatically when a new enemy turn starts.
    public override void OnTurnStarted(
        Enemy enemy,
        EnemyAIContext context)
    {
        if (!context.alternatingBossInitialized)
        {
            context.bossNormalTurn = startWithNormalTurn;
            context.alternatingBossInitialized = true;
        }
        else
        {
            context.bossNormalTurn = !context.bossNormalTurn;
        }

        Debug.Log(
            $"{enemy.name} begins " +
            $"{(context.bossNormalTurn ? "NORMAL" : "RAM")} turn."
        );
    }

    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context)
    {
        if (normalBehavior == null || ramBehavior == null)
        {
            Debug.LogError(
                $"Alternating Boss AI '{name}' is missing a behavior."
            );

            return null;
        }

        EnemyAIBehavior selectedBehavior =
            context.bossNormalTurn
                ? normalBehavior
                : ramBehavior;

        return selectedBehavior.DecideAction(
            enemy,
            battleManager,
            context
        );
    }
}