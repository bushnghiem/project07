using UnityEngine;

public class StateMachineAI : EnemyAIBase
{
    public BattleManager battleManager;

    [Header("Behavior")]
    public float preferredShootDistancePercent = 0.7f;

    [Header("Accuracy")]
    [Range(0f, 20f)]
    public float aimErrorAngle = 6f;

    private EnemyState idleState = new IdleState();
    private EnemyState moveState = new MoveState();
    private EnemyState attackState = new AttackState();
    private EnemyState orbitState = new OrbitState();

    private void Awake()
    {
        if (battleManager == null)
            battleManager = FindFirstObjectByType<BattleManager>();
    }

    public override void TakeTurn(Enemy enemy)
    {
        EnemyState chosenState = DecideState(enemy);

        if (chosenState == null)
        {
            Debug.LogWarning("No State");
            enemy.EndTurn();
            return;
        }

        Debug.Log($"[{enemy.name}] Using {chosenState.GetType().Name}");

        chosenState.Execute(enemy, this);
    }

    private EnemyState DecideState(Enemy enemy)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);

        if (target == null)
            return idleState;

        float distance = Vector3.Distance(enemy.transform.position, target.transform.position);
        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);
        bool hasLOS = EnemyAIUtility.HasLineOfSight(enemy, target);

        float desiredDistance = maxShotRange * preferredShootDistancePercent;

        if (hasLOS && distance <= maxShotRange)
        {
            if (distance < desiredDistance * 0.8f)
                return orbitState;

            return attackState;
        }

        return moveState;
    }
}