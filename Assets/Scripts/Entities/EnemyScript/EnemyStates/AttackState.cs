using UnityEngine;

public class AttackState : EnemyState
{
    public override void Execute(Enemy enemy, StateMachineAI ai)
    {
        var target = EnemyAIUtility.GetClosestPlayer(enemy, ai.battleManager);

        if (target == null)
        {
            enemy.EndTurn();
            return;
        }

        Vector3 direction = target.transform.position - enemy.transform.position;
        direction.y = 0;
        direction.Normalize();

        float distance = Vector3.Distance(enemy.transform.position, target.transform.position);
        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);

        float power = distance / maxShotRange;

        // Add error
        power += Random.Range(-0.15f, 0.2f);
        power = Mathf.Clamp01(power);

        float error = ai.aimErrorAngle * (distance / maxShotRange);
        direction = EnemyAIUtility.ApplyAimError(direction, error);

        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(true);
        enemy.clickAndFlingComp.ExecuteFling(direction, power);

        enemy.Shoot();
        enemy.EndTurn();
    }
}