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

        float distance = Vector3.Distance(enemy.Position, target.Position);
        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);
        float desiredDistance = maxShotRange * ai.preferredShootDistancePercent;
        float maxMoveRange = EnemyAIUtility.EstimateMoveRange(enemy);

        Vector3 bestDir = Vector3.zero;
        float bestScore = float.MinValue;
        int samples = 12;

        for (int i = 0; i < samples; i++)
        {
            float angle = (360f / samples) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 futurePos = enemy.Position + dir * maxMoveRange;

            float score = 0f;

            float futureDist = Vector3.Distance(futurePos, target.Position);
            score += 2f * (1f - Mathf.Abs(futureDist - desiredDistance) / desiredDistance);

            float planetPenalty = 0f;
            foreach (var p in GameObject.FindGameObjectsWithTag("Planet"))
            {
                float d = Vector3.Distance(futurePos, p.transform.position);
                if (d < 15f) planetPenalty += 1f - (d / 15f);
            }
            score -= planetPenalty * 5f;

            float allyPenalty = 0f;
            foreach (var hit in Physics.OverlapSphere(futurePos, 4f))
            {
                if (hit.gameObject == enemy.gameObject) continue;
                if (hit.GetComponent<Enemy>() != null)
                {
                    float d = Vector3.Distance(futurePos, hit.transform.position);
                    allyPenalty += 1f - (d / 4f);
                }
            }
            score -= allyPenalty * 4f;

            if (EnemyAIUtility.HasLineOfSight(enemy, target)) score += 0.5f;

            if (score > bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }

        if (bestDir == Vector3.zero)
            bestDir = (target.Position - enemy.Position).normalized;

        bestDir = EnemyAIUtility.GetSteeredDirection(enemy, bestDir);
        float movePower = Mathf.Clamp((distance - desiredDistance) / maxMoveRange, 0.4f, 1f);

        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(false);
        enemy.clickAndFlingComp.ExecuteFling(bestDir, movePower);

        enemy.Move();
        enemy.EndTurn();
    }
}