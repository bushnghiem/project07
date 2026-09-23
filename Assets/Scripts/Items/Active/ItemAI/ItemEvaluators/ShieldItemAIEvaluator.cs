using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Items/Shield Evaluator")]
public class ShieldItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    [Tooltip("Base value of adding shield.")]
    public float shieldValue = 5f;

    [Tooltip("Additional value for shielding injured allies.")]
    public float lowHealthBonus = 8f;

    [Tooltip("Additional value when the target has little/no existing shield.")]
    public float lowShieldBonus = 5f;

    [Tooltip("Shield amount considered to be 'enough'.")]
    public float desiredShield = 3f;

    [Header("Targeting")]
    [Tooltip("Small bonus for shielding yourself.")]
    public float selfShieldBonus = 1f;

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
            $"SHIELD EVALUATION START: " +
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

        ShieldItem shieldItem =
            item.itemData as ShieldItem;

        if (shieldItem == null)
        {
            DebugLog(
                "FAILED: item is not a ShieldItem");
            return null;
        }

        DebugLog(
            $"Shield item confirmed. " +
            $"Shield Amount={shieldItem.shieldAmount}");

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
            {
                DebugLog("Skipping null ally.");
                continue;
            }

            Enemy ally =
                unit as Enemy;

            if (ally == null)
            {
                DebugLog(
                    $"Skipping {unit}: not an Enemy.");
                continue;
            }

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
            // SHIELD
            // --------------------------------------------------

            int currentShield =
                ally.CurrentShield;

            float shieldNeed =
                Mathf.Clamp01(
                    1f -
                    ((float)currentShield /
                    desiredShield));

            DebugLog(
                $"{ally.name}: " +
                $"Current Shield={currentShield}, " +
                $"Shield Need={shieldNeed:F2}");

            // --------------------------------------------------
            // HEALTH
            // --------------------------------------------------

            if (ally.MaxHealth <= 0f)
            {
                DebugLog(
                    $"Skipping {ally.name}: " +
                    $"MaxHealth <= 0");
                continue;
            }

            float healthPercent =
                ally.CurrentHealth /
                ally.MaxHealth;

            float missingHealth =
                1f - healthPercent;

            DebugLog(
                $"{ally.name}: " +
                $"Health={ally.CurrentHealth:F1}/" +
                $"{ally.MaxHealth:F1}, " +
                $"Missing={missingHealth:P1}");

            // --------------------------------------------------
            // SCORE
            // --------------------------------------------------

            float score = 0f;

            score +=
                shieldNeed *
                shieldItem.shieldAmount *
                shieldValue;

            score +=
                missingHealth *
                lowHealthBonus;

            score +=
                shieldNeed *
                lowShieldBonus;

            if (ally == enemy)
            {
                score += selfShieldBonus;

                DebugLog(
                    $"Self Shield Bonus: " +
                    $"+{selfShieldBonus:F2}");
            }

            // --------------------------------------------------
            // AP PENALTY
            // --------------------------------------------------

            float apPenalty =
                shieldItem.apCost *
                apPenaltyMultiplier;

            score -= apPenalty;

            DebugLog(
                $"{ally.name} SCORE: " +
                $"ShieldNeed={shieldNeed:F2}, " +
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
                "FAILED: No valid shield target found.");

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
            $"FINAL SHIELD SCORE = {bestScore:F2}");

        DebugLog(
            $"BEST TARGET = {bestTarget.name}");

        DebugLog("SHIELD EVALUATION END");
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
            $"[Shield AI] {message}");
    }
}
