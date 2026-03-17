using UnityEngine;

public class MoveState : EnemyState
{
    public override void Execute(Enemy enemy, StateMachineAI ai)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, ai.battleManager);

        if (target == null)
        {
            enemy.EndTurn();
            return;
        }

        Vector3 targetDir = (target.transform.position - enemy.transform.position).normalized;

        Vector3 planetAvoid = EnemyAIUtility.GetPlanetAvoidance(enemy);
        Vector3 unitAvoid = EnemyAIUtility.GetUnitAvoidance(enemy);

        float targetWeight = 1f;
        float planetWeight = 4f;
        float unitWeight = 4f;

        Vector3 finalDir = targetDir * targetWeight
                         + planetAvoid * planetWeight
                         + unitAvoid * unitWeight;

        if (finalDir.sqrMagnitude < 0.01f)
            finalDir = targetDir;

        finalDir.Normalize();

        finalDir = EnemyAIUtility.GetSteeredDirection(enemy, finalDir);

        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);
        float desiredDistance = maxShotRange * ai.preferredShootDistancePercent;
        float distance = Vector3.Distance(enemy.transform.position, target.transform.position);
        float maxMoveRange = EnemyAIUtility.EstimateMoveRange(enemy);

        float movePower = Mathf.Clamp01((distance - desiredDistance) / maxMoveRange);
        movePower *= Random.Range(0.9f, 1.1f);
        movePower = Mathf.Clamp01(movePower);

        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(false);
        enemy.clickAndFlingComp.ExecuteFling(finalDir, movePower);

        enemy.Move();
        enemy.EndTurn();
    }
}