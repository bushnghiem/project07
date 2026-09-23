using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Items/Positive Status Effect Evaluator")]
public class PositiveStatusEffectItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    [Tooltip("Base value of successfully applying the positive effect.")]
    public float effectValue = 10f;

    [Tooltip("Additional value based on missing health.")]
    public float lowHealthBonus = 5f;

    [Tooltip("Additional value for applying the effect to yourself.")]
    public float selfTargetBonus = 1f;

    [Header("Requirements")]
    [Tooltip("Minimum health percentage missing before the AI considers the target.")]
    [Range(0f, 1f)]
    public float minimumMissingHealth = 0f;

    [Header("Cost")]
    public float apPenaltyMultiplier = 0.5f;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    public override AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager)
    {
        DebugLog("==================================================");
        DebugLog(
            $"POSITIVE STATUS EVALUATION START: " +
            $"Enemy={enemy?.name}, " +
            $"Item={item?.itemData?.name}");

        // --------------------------------------------------
        // BASIC VALIDATION
        // --------------------------------------------------

        if (enemy == null)
        {
            DebugLog("FAILED: enemy == null");
            return null;
        }

        if (item == null)
        {
            DebugLog("FAILED: item == null");
            return null;
        }

        if (item.itemData == null)
        {
            DebugLog("FAILED: item.itemData == null");
            return null;
        }

        if (battleManager == null)
        {
            DebugLog("FAILED: battleManager == null");
            return null;
        }

        // --------------------------------------------------
        // ITEM TYPE
        // --------------------------------------------------

        StatusEffectItem statusItem =
            item.itemData as StatusEffectItem;

        if (statusItem == null)
        {
            DebugLog(
                "FAILED: item is not a StatusEffectItem");
            return null;
        }

        if (statusItem.statusEffect == null)
        {
            DebugLog(
                "FAILED: statusEffect == null");
            return null;
        }

        DebugLog(
            $"Positive Status Effect confirmed: " +
            $"{statusItem.statusEffect.name}");

        DebugLog(
            $"Stacks={statusItem.stacks}");

        // --------------------------------------------------
        // CAN USE
        // --------------------------------------------------

        if (!item.CanUse(enemy))
        {
            DebugLog(
                "FAILED: item.CanUse(enemy) returned FALSE");
            return null;
        }

        DebugLog("CanUse = TRUE");

        // --------------------------------------------------
        // GET ALLIES
        // --------------------------------------------------

        List<Unit> allies =
            battleManager.allEnemies;

        if (allies == null || allies.Count == 0)
        {
            DebugLog(
                "FAILED: No allies found.");

            return null;
        }

        DebugLog(
            $"Allies found: {allies.Count}");

        // --------------------------------------------------
        // FIND BEST TARGET
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
            {
                DebugLog(
                    $"Skipping {ally.name}: DEAD");
                continue;
            }

            // --------------------------------------------------
            // RANGE
            // --------------------------------------------------

            float distance =
                Vector3.Distance(
                    enemy.Position,
                    ally.Position);

            DebugLog(
                $"Checking {ally.name}: " +
                $"Distance={distance:F2}, " +
                $"Range={item.itemData.range:F2}");

            if (distance > item.itemData.range)
            {
                DebugLog(
                    $"Skipping {ally.name}: OUT OF RANGE");
                continue;
            }

            // --------------------------------------------------
            // HEALTH
            // --------------------------------------------------

            float missingHealth = 0f;

            if (ally.MaxHealth > 0f)
            {
                float healthPercent =
                    ally.CurrentHealth /
                    ally.MaxHealth;

                missingHealth =
                    1f - healthPercent;
            }

            if (missingHealth < minimumMissingHealth)
            {
                DebugLog(
                    $"Skipping {ally.name}: " +
                    $"not enough missing health.");

                continue;
            }

            // --------------------------------------------------
            // SCORE
            // --------------------------------------------------

            float score =
                effectValue;

            score +=
                missingHealth *
                lowHealthBonus;

            if (ally == enemy)
            {
                score += selfTargetBonus;

                DebugLog(
                    $"Self Target Bonus: " +
                    $"+{selfTargetBonus:F2}");
            }

            // --------------------------------------------------
            // AP PENALTY
            // --------------------------------------------------

            float apPenalty =
                statusItem.apCost *
                apPenaltyMultiplier;

            score -= apPenalty;

            DebugLog(
                $"{ally.name}: " +
                $"MissingHealth={missingHealth:P1}, " +
                $"Score={score:F2}");

            // --------------------------------------------------
            // BEST TARGET
            // --------------------------------------------------

            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = ally;

                DebugLog(
                    $"NEW BEST TARGET: " +
                    $"{ally.name}, " +
                    $"Score={score:F2}");
            }
        }

        // --------------------------------------------------
        // NO TARGET
        // --------------------------------------------------

        if (bestTarget == null)
        {
            DebugLog(
                "FAILED: No valid target found.");

            return null;
        }

        // --------------------------------------------------
        // RETURN
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = bestTarget
            };

        DebugLog(
            $"FINAL POSITIVE STATUS SCORE = " +
            $"{bestScore:F2}");

        DebugLog(
            $"BEST TARGET = {bestTarget.name}");

        DebugLog(
            "POSITIVE STATUS EVALUATION END");

        DebugLog("==================================================");

        return new AIItemEvaluation(
            bestScore,
            targetData);
    }

    private void DebugLog(string message)
    {
        if (!enableDebugLogs)
            return;

        Debug.Log(
            $"[PositiveStatus AI] {message}");
    }
}
