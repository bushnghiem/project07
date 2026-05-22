using UnityEngine;

public class RamStateMachineAI : EnemyAIBase
{
    public float ramDistanceThreshold = 3f;

    [Header("Personality")]
    [Range(0f, 1f)] public float aggression = 1f;
    [Range(0f, 1f)] public float decisiveness = 0.8f;

    private EnemyState moveState = new RamMoveState();
    private EnemyState ramState = new RamAttackState();
    private EnemyState idleState = new IdleState();

    private EnemyState lastState;
    private UnitActionExecutor executor;

    private void Awake()
    {
        if (battleManager == null)
            battleManager = FindFirstObjectByType<BattleManager>();

        executor = FindFirstObjectByType<UnitActionExecutor>();
    }

    public override void TakeTurn(Enemy enemy)
    {
        EnemyState state = DecideState(enemy);

        if (state == null)
        {
            enemy.EndTurn();
            return;
        }

        UnitAction action = state.DecideAction(enemy, this);

        if (action == null)
        {
            enemy.EndTurn();
            return;
        }

        executor.Execute(action);
        lastState = state;
    }

    private EnemyState DecideState(Enemy enemy)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);
        if (target == null) return idleState;

        float distance = Vector3.Distance(enemy.Position, target.Position);

        // If close enough → RAM
        if (distance <= ramDistanceThreshold)
            return ramState;

        return moveState;
    }
}