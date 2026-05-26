using UnityEngine;

public class MoveState : EnemyState
{
    private float preferredShootDistancePercent;

    public MoveState(float preferredShootDistancePercent)
    {
        this.preferredShootDistancePercent =
            preferredShootDistancePercent;
    }

    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager)
    {
        var target =
            EnemyAIUtility.GetClosestPlayer(
                enemy,
                battleManager);

        if (target == null)
            return null;

        float distance =
            Vector3.Distance(
                enemy.Position,
                target.Position);

        float maxShotRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        float desiredDistance =
            maxShotRange *
            preferredShootDistancePercent;

        float maxMoveRange =
            EnemyAIUtility.EstimateMoveRange(enemy);

        Vector3 bestDir = Vector3.zero;
        float bestScore = float.MinValue;

        int samples = 12;

        for (int i = 0; i < samples; i++)
        {
            float angle = (360f / samples) * i;

            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            Vector3 futurePos = enemy.Position + dir * maxMoveRange;

            float futureDist = Vector3.Distance(futurePos, target.Position);

            float score = 2f * (1f - Mathf.Abs(futureDist - desiredDistance) / desiredDistance);

            foreach (var p in GameObject.FindGameObjectsWithTag("Planet"))
            {
                float d = Vector3.Distance(futurePos, p.transform.position);

                if (d < 15f)
                    score -= (1f - d / 15f) * 5f;
            }

            foreach (var hit in Physics.OverlapSphere(futurePos, 4f))
            {
                if (hit.GetComponent<Enemy>() != null &&
                    hit.gameObject != enemy.gameObject)
                {
                    float d = Vector3.Distance(futurePos, hit.transform.position);

                    score -= (1f - d / 4f) * 4f;
                }
            }

            if (EnemyAIUtility.HasLineOfSight(enemy, target))
                score += 0.5f;

            if (score > bestScore)
            {
                bestScore = score;
                bestDir = dir;
            }
        }

        if (bestDir == Vector3.zero)
        {
            bestDir = (target.Position - enemy.Position).normalized;
        }

        bestDir = EnemyAIUtility.GetSteeredDirection(enemy, bestDir);

        float movePower = Mathf.Clamp((distance - desiredDistance) / maxMoveRange, 0.4f, 1f);

        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Move,
            direction = bestDir,
            powerPercent = movePower
        };
    }
}