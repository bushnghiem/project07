using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Items/Repulsor Beam Evaluator")]
public class RepulsorBeamItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    public float playerHitWeight = 5f;
    public float multipleTargetBonus = 4f;
    public float closeTargetWeight = 4f;

    [Header("Beam")]
    public float sampleAngleStep = 15f;

    [Header("Preferred Distance")]
    public float preferredDistance = 10f;

    public override AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager)
    {
        if (enemy == null || item == null)
            return null;

        if (!item.CanUse(enemy))
            return null;

        if (battleManager == null)
            return null;

        List<Unit> players =
            battleManager.allPlayers;

        if (players == null || players.Count == 0)
            return null;

        float bestScore = float.MinValue;
        Vector3 bestDirection = Vector3.forward;

        for (
            float angle = 0f;
            angle < 360f;
            angle += sampleAngleStep)
        {
            Vector3 direction =
                Quaternion.Euler(
                    0f,
                    angle,
                    0f) *
                Vector3.forward;

            float score =
                EvaluateDirection(
                    enemy,
                    direction,
                    players,
                    item);

            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = direction;
            }
        }

        if (bestScore == float.MinValue)
            return null;

        ItemTargetData targetData =
            new ItemTargetData
            {
                direction = bestDirection
            };

        return new AIItemEvaluation(
            bestScore,
            targetData);
    }

    private float EvaluateDirection(
        Enemy enemy,
        Vector3 direction,
        List<Unit> players,
        ActiveItemInstance item)
    {
        int hitCount = 0;

        float score = 0f;

        float cosThreshold =
            Mathf.Cos(
                item.itemData.coneAngle *
                0.5f *
                Mathf.Deg2Rad);

        foreach (Unit unit in players)
        {
            Player player =
                unit as Player;

            if (player == null || player.isDead)
                continue;

            Vector3 toTarget =
                player.Position -
                enemy.Position;

            toTarget.y = 0f;

            float distance =
                toTarget.magnitude;

            if (distance < 0.01f)
                continue;

            if (distance > item.itemData.effectRadius)
                continue;

            Vector3 toTargetDirection =
                toTarget.normalized;

            float dot =
                Vector3.Dot(
                    direction,
                    toTargetDirection);

            if (dot < cosThreshold)
                continue;

            hitCount++;

            score += playerHitWeight;

            // Repulsor becomes more valuable when enemies
            // are dangerously close.
            if (distance < preferredDistance)
            {
                float danger =
                    1f -
                    Mathf.Clamp01(
                        distance /
                        preferredDistance);

                score +=
                    danger *
                    closeTargetWeight;
            }
        }

        if (hitCount == 0)
            return float.MinValue;

        if (hitCount > 1)
        {
            score +=
                (hitCount - 1) *
                multipleTargetBonus;
        }

        score -=
            item.itemData.apCost *
            0.5f;

        return score;
    }
}
