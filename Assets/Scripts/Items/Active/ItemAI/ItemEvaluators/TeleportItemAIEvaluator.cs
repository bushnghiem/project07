using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Items/Teleport Evaluator")]
public class TeleportItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    public float distanceWeight = 0.5f;
    public float positionImprovementWeight = 5f;
    public float escapeWeight = 5f;

    [Header("Teleport Requirements")]
    [Tooltip("Teleport must move at least this far to be considered.")]
    public float minimumTeleportDistance = 4f;

    [Tooltip(
        "Teleport must travel at least this much farther " +
        "than normal movement to be worthwhile.")]
    public float movementAdvantage = 2f;

    [Header("Combat Position")]
    [Range(0.1f, 1f)]
    public float preferredShootDistancePercent = 0.7f;

    public override AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager)
    {
        if (enemy == null ||
            item == null ||
            battleManager == null)
        {
            return null;
        }

        if (!item.CanUse(enemy))
            return null;

        Player target =
            EnemyAIUtility.GetClosestPlayer(
                enemy,
                battleManager);

        if (target == null)
            return null;

        // --------------------------------------------------
        // FIND BEST DESTINATION
        // --------------------------------------------------

        Vector3 bestPosition;
        float bestScore;

        if (!FindBestTeleportPosition(
                enemy,
                target,
                item,
                out bestPosition,
                out bestScore))
        {
            return null;
        }

        // --------------------------------------------------
        // RETURN RESULT
        // --------------------------------------------------

        Debug.Log(
            $"{enemy.name} TELEPORT AI: " +
            $"Destination={bestPosition}, " +
            $"Score={bestScore:F2}");

        return new AIItemEvaluation(
            bestScore,
            new ItemTargetData
            {
                targetPosition = bestPosition
            });
    }

    private bool FindBestTeleportPosition(
        Enemy enemy,
        Player target,
        ActiveItemInstance item,
        out Vector3 bestPosition,
        out float bestScore)
    {
        bestPosition = Vector3.zero;
        bestScore = float.MinValue;

        Vector3 toTarget =
            target.Position -
            enemy.Position;

        toTarget.y = 0f;

        if (toTarget.sqrMagnitude < 0.01f)
            return false;

        toTarget.Normalize();

        float teleportRange =
            item.itemData.range;

        float normalMoveRange =
            EnemyAIUtility.EstimateMoveRange(enemy);

        /*
         * We test several distances instead of always
         * teleporting the full range.
         */
        float[] distances =
        {
            teleportRange,
            teleportRange * 0.75f,
            teleportRange * 0.5f,
            teleportRange * 0.35f
        };

        foreach (float distance in distances)
        {
            if (distance < minimumTeleportDistance)
                continue;

            /*
             * If normal movement can accomplish almost the
             * same thing, teleport isn't worthwhile.
             */
            if (distance <
                normalMoveRange + movementAdvantage)
            {
                continue;
            }

            Vector3 destination =
                enemy.Position +
                toTarget *
                distance;

            destination.y =
                enemy.Position.y;

            // --------------------------------------------------
            // VALID TELEPORT LOCATION
            // --------------------------------------------------

            if (enemy is UnitBase unit)
            {
                if (!unit.CanTeleportTo(destination))
                    continue;
            }

            // --------------------------------------------------
            // SCORE DESTINATION
            // --------------------------------------------------

            float score =
                ScoreDestination(
                    enemy,
                    target,
                    item,
                    destination,
                    distance);

            if (score > bestScore)
            {
                bestScore = score;
                bestPosition = destination;
            }
        }

        return bestScore > float.MinValue;
    }

    private float ScoreDestination(
        Enemy enemy,
        Player target,
        ActiveItemInstance item,
        Vector3 destination,
        float teleportDistance)
    {
        float score = 0f;

        // --------------------------------------------------
        // TELEPORT DISTANCE
        // --------------------------------------------------

        score +=
            teleportDistance *
            distanceWeight;

        // --------------------------------------------------
        // COMBAT RANGE
        // --------------------------------------------------

        float maxShotRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        float desiredDistance =
            maxShotRange *
            preferredShootDistancePercent;

        float currentDistance =
            Vector3.Distance(
                enemy.Position,
                target.Position);

        float futureDistance =
            Vector3.Distance(
                destination,
                target.Position);

        float currentError =
            Mathf.Abs(
                currentDistance -
                desiredDistance);

        float futureError =
            Mathf.Abs(
                futureDistance -
                desiredDistance);

        float improvement =
            currentError -
            futureError;

        score +=
            improvement *
            positionImprovementWeight;

        // --------------------------------------------------
        // ESCAPE
        // --------------------------------------------------

        /*
         * If the enemy is extremely close to the player,
         * teleporting away can be valuable.
         */
        if (currentDistance <
            maxShotRange * 0.4f)
        {
            score += escapeWeight;
        }

        // --------------------------------------------------
        // AP COST
        // --------------------------------------------------

        score -=
            item.itemData.apCost *
            0.5f;

        return score;
    }
}
