using UnityEngine;

[CreateAssetMenu(menuName = "AI/Sniper AI")]
public class SniperAIBehavior : ItemUsingAIBehavior
{
    [Header("Sniper Distance")]

    [Tooltip("Preferred shooting distance as a percentage of maximum weapon range.")]
    [Range(0f, 1f)]
    public float sniperDistancePercent = 0.85f;

    [Tooltip("Distance percentage below which the sniper strongly wants to retreat.")]
    [Range(0f, 1f)]
    public float minimumDistancePercent = 0.45f;


    [Header("Retreat Behavior")]

    [Tooltip("How strongly the sniper prioritizes getting away when too close.")]
    public float retreatUrgency = 8f;

    [Tooltip("How much actual distance gained from movement is rewarded.")]
    public float retreatMovementValue = 0.5f;

    [Tooltip("Extra reward for escaping when extremely close.")]
    public float emergencyRetreatBonus = 10f;


    [Header("Close Range Shooting")]

    [Tooltip("Penalty for shooting when the target is inside the minimum sniper distance.")]
    public float closeRangeShootPenalty = 8f;


    // ============================================================
    // SHOOT
    // ============================================================

    protected override float ScoreShoot(
        UnitAction action,
        Enemy enemy,
        Player target)
    {
        // Start with all of the normal ItemUsingAIBehavior
        // shooting logic.
        float score =
            base.ScoreShoot(
                action,
                enemy,
                target);

        float distance =
            Vector3.Distance(
                enemy.Position,
                target.Position);

        float maxRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        if (maxRange <= 0f)
            return score;


        // --------------------------------------------------------
        // DISTANCE THRESHOLDS
        // --------------------------------------------------------

        float minimumDistance =
            maxRange *
            minimumDistancePercent;


        // --------------------------------------------------------
        // PENALIZE SHOOTING WHEN TOO CLOSE
        // --------------------------------------------------------

        if (distance < minimumDistance)
        {
            float closeness =
                1f -
                Mathf.Clamp01(
                    distance / minimumDistance);

            score -=
                closeRangeShootPenalty *
                closeness;
        }


        return score;
    }


    // ============================================================
    // MOVE
    // ============================================================

    protected override float ScoreMove(
        UnitAction action,
        Enemy enemy,
        Player target,
        BattleManager battleManager)
    {
        // Start with all of the normal movement scoring.
        float score =
            base.ScoreMove(
                action,
                enemy,
                target,
                battleManager);


        float distance =
            Vector3.Distance(
                enemy.Position,
                target.Position);

        float maxRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        if (maxRange <= 0f)
            return score;


        // --------------------------------------------------------
        // DISTANCE THRESHOLDS
        // --------------------------------------------------------

        float minimumDistance =
            maxRange *
            minimumDistancePercent;

        float preferredDistance =
            maxRange *
            sniperDistancePercent;


        // --------------------------------------------------------
        // HOW MUCH WILL THIS MOVE CHANGE OUR DISTANCE?
        // --------------------------------------------------------

        float distanceChange =
            EnemyAIUtility.GetMovementDistanceChange(
                enemy,
                target,
                action);


        // Positive distanceChange:
        //
        //     moving AWAY from target
        //
        // Negative distanceChange:
        //
        //     moving TOWARD target
        //
        // Zero:
        //
        //     mostly sideways


        // ========================================================
        // TOO CLOSE
        // ========================================================

        if (distance < minimumDistance)
        {
            float danger =
                1f -
                Mathf.Clamp01(
                    distance / minimumDistance);


            // ----------------------------------------------------
            // REWARD RETREATING
            // ----------------------------------------------------

            if (distanceChange > 0f)
            {
                score +=
                    distanceChange *
                    retreatMovementValue *
                    retreatUrgency *
                    danger;
            }


            // ----------------------------------------------------
            // PENALIZE MOVING CLOSER
            // ----------------------------------------------------

            else if (distanceChange < 0f)
            {
                score -=
                    Mathf.Abs(distanceChange) *
                    retreatUrgency;
            }


            // ----------------------------------------------------
            // EMERGENCY RETREAT
            // ----------------------------------------------------

            if (distance < minimumDistance * 0.5f)
            {
                if (distanceChange > 0f)
                {
                    score +=
                        emergencyRetreatBonus;
                }
            }
        }


        // ========================================================
        // BELOW PREFERRED SNIPER RANGE
        // ========================================================

        if (distance < preferredDistance)
        {
            float needDistance =
                1f -
                Mathf.Clamp01(
                    distance / preferredDistance);


            if (distanceChange > 0f)
            {
                score +=
                    distanceChange *
                    retreatMovementValue *
                    needDistance;
            }
        }


        return score;
    }
}
