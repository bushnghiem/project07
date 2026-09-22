using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Items/Heal Evaluator")]
public class HealItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    public float missingHealthWeight = 10f;

    [Tooltip("Small bonus for healing yourself.")]
    public float selfHealBonus = 1f;

    [Tooltip("Minimum amount of missing health required before healing.")]
    [Range(0f, 1f)]
    public float minimumMissingHealth = 0.2f;

    public override AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager)
    {
        // --------------------------------------------------
        // BASIC VALIDATION
        // --------------------------------------------------

        if (item == null)
            return null;

        if (!item.CanUse(enemy))
            return null;

        if (battleManager == null)
            return null;

        // --------------------------------------------------
        // GET ALLIES
        // --------------------------------------------------

        List<Unit> allies =
            battleManager.allEnemies;

        if (allies == null || allies.Count == 0)
            return null;

        // --------------------------------------------------
        // FIND BEST HEAL TARGET
        // --------------------------------------------------

        Enemy bestTarget = null;
        float bestScore = float.MinValue;

        foreach (Unit unit in allies)
        {
            if (unit == null)
                continue;

            Enemy ally =
                unit as Enemy;

            if (ally == null)
                continue;

            if (ally.isDead)
                continue;

            // --------------------------------------------------
            // RANGE CHECK
            // --------------------------------------------------

            float distance =
                Vector3.Distance(
                    enemy.Position,
                    ally.Position);

            if (distance > item.itemData.range)
            {
                Debug.Log(
                    $"{enemy.name} HEAL AI: " +
                    $"{ally.name} is out of range. " +
                    $"Distance={distance:F2}, " +
                    $"Range={item.itemData.range:F2}");

                continue;
            }

            // --------------------------------------------------
            // HEALTH CHECK
            // --------------------------------------------------

            if (ally.MaxHealth <= 0f)
                continue;

            float healthPercent =
                ally.CurrentHealth /
                ally.MaxHealth;

            float missingHealth =
                1f - healthPercent;

            // Don't waste the item on healthy allies.
            if (missingHealth < minimumMissingHealth)
                continue;

            // --------------------------------------------------
            // SCORE
            // --------------------------------------------------

            float score =
                missingHealth *
                missingHealthWeight;

            // Small preference for healing yourself.
            if (ally == enemy)
            {
                score += selfHealBonus;
            }

            // AP cost penalty.
            score -=
                item.itemData.apCost *
                0.5f;

            Debug.Log(
                $"{enemy.name} HEAL AI: " +
                $"Checking {ally.name}, " +
                $"Distance={distance:F2}, " +
                $"Health={ally.CurrentHealth:F1}/" +
                $"{ally.MaxHealth:F1}, " +
                $"Missing={missingHealth:P0}, " +
                $"Score={score:F2}");

            // --------------------------------------------------
            // BEST TARGET
            // --------------------------------------------------

            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = ally;
            }
        }

        // --------------------------------------------------
        // NO VALID TARGET
        // --------------------------------------------------

        if (bestTarget == null)
        {
            Debug.Log(
                $"{enemy.name} HEAL AI: " +
                $"No injured ally in range.");

            return null;
        }

        Debug.Log(
            $"{enemy.name} HEAL AI: " +
            $"BEST TARGET = {bestTarget.name}, " +
            $"Score={bestScore:F2}");

        // --------------------------------------------------
        // CREATE TARGET DATA
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = bestTarget
            };

        // --------------------------------------------------
        // RETURN SCORE + TARGET
        // --------------------------------------------------

        return new AIItemEvaluation(
            bestScore,
            targetData);
    }
}
