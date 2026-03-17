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

        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);
        float desiredDistance = maxShotRange * ai.preferredShootDistancePercent;

        Vector3 dir = EnemyAIUtility.GetOrbitDirection(enemy, target, desiredDistance);
        dir = EnemyAIUtility.GetSteeredDirection(enemy, dir);

        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(false);
        enemy.clickAndFlingComp.ExecuteFling(dir, 0.6f);

        enemy.Move();
        enemy.EndTurn();
    }
}