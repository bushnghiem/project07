using UnityEngine;

public class OrbitState : EnemyState
{
    public override void Execute(Enemy enemy, StateMachineAI ai)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, ai.battleManager);
        if (target == null)
        {
            enemy.EndTurn();
            return;
        }

        float maxRange = EnemyAIUtility.EstimateShotRange(enemy);
        float desiredDistance = maxRange * ai.preferredShootDistancePercent;

        float distance = Vector3.Distance(enemy.Position, target.Position);
        float error = Mathf.Abs(distance - desiredDistance);

        Vector3 dir = EnemyAIUtility.GetOrbitDirection(enemy, target, desiredDistance);
        dir = EnemyAIUtility.GetSteeredDirection(enemy, dir);

        float power = Mathf.Lerp(0.4f, 0.8f, error / desiredDistance);

        // Flip sometimes
        if (Random.value < 0.1f)
            enemy.orbitSide *= -1;

        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(false);
        enemy.clickAndFlingComp.ExecuteFling(dir, power);

        enemy.Move();
        enemy.EndTurn();
    }
}