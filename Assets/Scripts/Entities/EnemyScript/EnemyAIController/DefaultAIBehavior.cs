using UnityEngine;

[CreateAssetMenu(menuName = "AI/Default AI")]
public class DefaultAIBehavior : EnemyAIBehavior
{
    [Header("Behavior")]
    public float preferredShootDistancePercent = 0.7f;

    [Header("Personality")]
    [Range(0f, 1f)]
    public float aggression = 0.7f;

    [Range(0f, 1f)]
    public float orbitPreference = 0.5f;

    [Range(0f, 1f)]
    public float decisiveness = 0.7f;

    [Header("Accuracy")]
    [Range(0f, 20f)]
    public float aimErrorAngle = 6f;

    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context)
    {
        Player target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);

        if (target == null)
            return null;

        float distance = Vector3.Distance(enemy.Position, target.Position);

        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);

        bool hasLOS = EnemyAIUtility.HasLineOfSight(enemy, target);

        float desiredDistance = maxShotRange * preferredShootDistancePercent;

        float attack = 0f;
        float move = 0f;
        float orbit = 0f;

        float distance01 = Mathf.Clamp01(distance / maxShotRange);

        attack += hasLOS ? 2f : 0f;
        attack += (1f - distance01);

        move += distance01;

        orbit += 1f - Mathf.Abs(distance - desiredDistance) / desiredDistance;

        attack *= aggression;
        orbit *= orbitPreference;

        EnemyState chosen;

        if (attack > move && attack > orbit)
        {
            chosen = new AttackState(aimErrorAngle);
        }
        else if (orbit > move)
        {
            chosen = new OrbitState(desiredDistance);
        }
        else
        {
            chosen = new MoveState(preferredShootDistancePercent);
        }

        context.lastState = chosen;

        return chosen.DecideAction(enemy, battleManager);
    }
}