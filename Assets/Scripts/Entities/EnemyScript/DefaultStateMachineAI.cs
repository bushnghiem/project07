using UnityEngine;

public class StateMachineAI : EnemyAIBase
{
    public BattleManager battleManager;

    [Header("Behavior")]
    public float preferredShootDistancePercent = 0.7f;

    [Header("Personality")]
    [Range(0f, 1f)] public float aggression = 0.7f;
    [Range(0f, 1f)] public float orbitPreference = 0.5f;
    [Range(0f, 1f)] public float decisiveness = 0.7f;

    [Header("Accuracy")]
    [Range(0f, 20f)]
    public float aimErrorAngle = 6f;

    private EnemyState idleState = new IdleState();
    private EnemyState moveState = new MoveState();
    private EnemyState attackState = new AttackState();
    private EnemyState orbitState = new OrbitState();

    private EnemyState lastState;

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
            enemy.EndTurn();
            return;
        }

        chosenState.Execute(enemy, this);
        lastState = chosenState;
    }

    private EnemyState DecideState(Enemy enemy)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);
        if (target == null) return idleState;

        float distance = Vector3.Distance(enemy.Position, target.Position);
        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);

        bool hasLOS = EnemyAIUtility.HasLineOfSight(enemy, target);

        float desiredDistance = maxShotRange * preferredShootDistancePercent;

        float distance01 = Mathf.Clamp01(distance / maxShotRange);
        float closeness = 1f - Mathf.Clamp01(Mathf.Abs(distance - desiredDistance) / desiredDistance);
        float los = hasLOS ? 1f : 0f;

        float attack = 0f;
        float move = 0f;
        float orbit = 0f;

        // Calculate action scores
        attack += los * 1.5f;
        attack += (1f - distance01) * 1.2f;
        attack *= Mathf.Lerp(0.5f, 1.5f, aggression);

        move += distance01 * 1.2f;
        move += (1f - los);

        orbit += closeness * 1.5f;
        orbit += los * 0.5f;
        orbit *= Mathf.Lerp(0.5f, 1.5f, orbitPreference);

        Vector3 dirToTarget = (target.Position - enemy.Position);
        dirToTarget.y = 0;
        dirToTarget.Normalize();

        int blockedCount = 0;
        int samples = 2;

        for (int i = 0; i < samples; i++)
        {
            float testError = aimErrorAngle * (distance / maxShotRange);
            Vector3 testDir = EnemyAIUtility.ApplyAimError(dirToTarget, testError);

            if (EnemyAIUtility.IsShotBlockedByAlly(enemy, testDir, distance))
                blockedCount++;
        }

        float unsafeFactor = (float)blockedCount / samples;

        attack *= Mathf.Lerp(1f, 0.2f, unsafeFactor);

        float rand = 0.15f;
        attack += Random.Range(0f, rand);
        move += Random.Range(0f, rand);
        orbit += Random.Range(0f, rand);

        // "Stickiness" of actions
        if (lastState != null)
        {
            float bonus = decisiveness * 0.75f;
            if (lastState == attackState) attack += bonus;
            if (lastState == moveState) move += bonus;
            if (lastState == orbitState) orbit += bonus;
        }

        if (attack > move && attack > orbit) return attackState;
        if (orbit > move) return orbitState;
        return moveState;
    }

    public void ForceMove(Enemy enemy)
    {
        moveState.Execute(enemy, this);
        lastState = moveState;
    }
}