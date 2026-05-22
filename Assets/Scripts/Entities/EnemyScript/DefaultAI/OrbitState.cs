using UnityEngine;

public class OrbitState : EnemyState
{
    public override UnitAction DecideAction(Enemy enemy, EnemyAIBase ai)
    {
        var typedAI = ai as StateMachineAI;
        if (typedAI == null) return null;

        var target = EnemyAIUtility.GetClosestPlayer(enemy, typedAI.battleManager);
        if (target == null) return null;

        float maxRange = EnemyAIUtility.EstimateShotRange(enemy);
        float desiredDistance = maxRange * typedAI.preferredShootDistancePercent;

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