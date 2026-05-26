using UnityEngine;

public class OrbitState : EnemyState
{
    private float preferredShootDistancePercent;

    public OrbitState(float preferredShootDistancePercent)
    {
        this.preferredShootDistancePercent = preferredShootDistancePercent;
    }

    public override UnitAction DecideAction(Enemy enemy, BattleManager battleManager)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);

        if (target == null)
            return null;

        float maxRange = EnemyAIUtility.EstimateShotRange(enemy);

        float desiredDistance = maxRange * preferredShootDistancePercent;

        float distance = Vector3.Distance(enemy.Position, target.Position);

        Vector3 dir = EnemyAIUtility.GetOrbitDirection(enemy, target, desiredDistance);

        dir = EnemyAIUtility.GetSteeredDirection(enemy, dir);

        float error = Mathf.Abs(distance - desiredDistance);

        float power = Mathf.Lerp(0.4f, 0.8f, error / desiredDistance);

        if (Random.value < 0.1f)
            enemy.orbitSide *= -1;

        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Move,
            direction = dir,
            powerPercent = power
        };
    }
}