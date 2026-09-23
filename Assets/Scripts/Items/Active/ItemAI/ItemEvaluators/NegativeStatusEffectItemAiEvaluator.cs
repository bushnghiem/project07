using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Items/Negative Status Effect Evaluator")]
public class NegativeStatusEffectItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    [Tooltip("Base value for successfully applying the negative status.")]
    public float statusValue = 10f;

    [Tooltip("Additional value for applying the effect to a low-health target.")]
    public float lowHealthBonus = 5f;

    [Tooltip("Additional value for applying the effect to a high-health target.")]
    public float highHealthBonus = 2f;

    [Tooltip("Bonus for targets that are close to the enemy.")]
    public float closeRangeBonus = 2f;

    [Tooltip("How strongly the AI prefers targets that are already injured.")]
    public float injuredTargetWeight = 5f;

    [Header("Target Requirements")]
    [Tooltip("Minimum number of valid enemy targets required.")]
    public int minimumTargets = 1;

    [Tooltip("Don't use the item on enemies below this health percentage.")]
    [Range(0f, 1f)]
    public float minimumTargetHealthPercent = 0f;

    [Header("Range")]
    [Tooltip("Additional range used by the evaluator. " +
             "Set to 0 to use the item's range directly.")]
    public float additionalRange = 0f;

    [Header("Cost")]
    [Tooltip("Multiplier applied to the AP cost penalty.")]
    public float apCostWeight = 0.5f;

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
            $"NEGATIVE STATUS EVALUATION START: " +
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

        DebugLog(
            $"Item Data = {item.itemData.name}");

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

        DebugLog(
            $"Status item confirmed. " +
            $"Effect={statusItem.statusEffect}, " +
            $"Stacks={statusItem.stacks}, " +
            $"Range={statusItem.range}");

        // --------------------------------------------------
        // STATUS EFFECT VALIDATION
        // --------------------------------------------------

        if (statusItem.statusEffect == null)
        {
            DebugLog(
                "FAILED: statusItem.statusEffect == null");

            return null;
        }

        if (statusItem.stacks <= 0)
        {
            DebugLog(
                $"FAILED: stacks <= 0. " +
                $"Stacks={statusItem.stacks}");

            return null;
        }

        DebugLog("Status effect validation PASSED.");

        // --------------------------------------------------
        // CAN USE
        // --------------------------------------------------

        DebugLog(
            $"Checking CanUse: " +
            $"Cooldown={item.GetRemainingCooldown()}, " +
            $"Charges={enemy.CurrentCharges}/" +
            $"{statusItem.chargeCost}, " +
            $"AP={enemy.CurrentAP}/" +
            $"{statusItem.apCost}");

        if (!item.CanUse(enemy))
        {
            DebugLog(
                "FAILED: item.CanUse(enemy) returned FALSE");

            return null;
        }

        DebugLog("CanUse = TRUE");

        // --------------------------------------------------
        // GET ENEMIES
        // --------------------------------------------------

        if (battleManager.allPlayers == null)
        {
            DebugLog(
                "FAILED: battleManager.allPlayers == null");

            return null;
        }

        DebugLog(
            $"Players found: " +
            $"{battleManager.allPlayers.Count}");

        // --------------------------------------------------
        // RANGE
        // --------------------------------------------------

        float effectiveRange =
            statusItem.range + additionalRange;

        DebugLog(
            $"Effective Range = {effectiveRange:F2}");

        // --------------------------------------------------
        // FIND BEST TARGET
        // --------------------------------------------------

        Player bestTarget = null;
        float bestScore = float.MinValue;

        int validTargetCount = 0;

        foreach (Player player in battleManager.allPlayers)
        {
            if (player == null)
            {
                DebugLog("Skipping player: null");
                continue;
            }

            DebugLog(
                $"Checking player: {player.name}");

            // --------------------------------------------------
            // DEAD CHECK
            // --------------------------------------------------

            if (player.isDead)
            {
                DebugLog(
                    $"Skipping {player.name}: DEAD");

                continue;
            }

            // --------------------------------------------------
            // RANGE CHECK
            // --------------------------------------------------

            float distance =
                Vector3.Distance(
                    enemy.Position,
                    player.Position);

            DebugLog(
                $"{player.name}: " +
                $"Distance={distance:F2}, " +
                $"Range={effectiveRange:F2}");

            if (distance > effectiveRange)
            {
                DebugLog(
                    $"Skipping {player.name}: " +
                    $"OUTSIDE RANGE");

                continue;
            }

            // --------------------------------------------------
            // HEALTH CHECK
            // --------------------------------------------------

            if (player.MaxHealth <= 0f)
            {
                DebugLog(
                    $"Skipping {player.name}: " +
                    $"MaxHealth <= 0");

                continue;
            }

            float healthPercent =
                player.CurrentHealth /
                player.MaxHealth;

            DebugLog(
                $"{player.name}: " +
                $"Health={player.CurrentHealth:F1}/" +
                $"{player.MaxHealth:F1}, " +
                $"HealthPercent={healthPercent:P1}");

            if (healthPercent < minimumTargetHealthPercent)
            {
                DebugLog(
                    $"Skipping {player.name}: " +
                    $"Health below minimum target threshold.");

                continue;
            }

            validTargetCount++;

            // --------------------------------------------------
            // TARGET HEALTH
            // --------------------------------------------------

            float missingHealth =
                1f - healthPercent;

            // --------------------------------------------------
            // SCORE
            // --------------------------------------------------

            float score = 0f;

            // Base value for putting the status on somebody.
            score += statusValue;

            DebugLog(
                $"{player.name}: " +
                $"Base Status Value = {statusValue:F2}");

            // Prefer injured enemies if configured.
            float injuredScore =
                missingHealth *
                injuredTargetWeight;

            score += injuredScore;

            DebugLog(
                $"{player.name}: " +
                $"Injured Target Score = " +
                $"{missingHealth:F2} * " +
                $"{injuredTargetWeight:F2} = " +
                $"{injuredScore:F2}");

            // Additional low-health bonus.
            if (healthPercent <= 0.5f)
            {
                score += lowHealthBonus;

                DebugLog(
                    $"{player.name}: " +
                    $"Low Health Bonus = " +
                    $"{lowHealthBonus:F2}");
            }

            // Additional value for healthy targets.
            // Useful for effects that are valuable before
            // the target becomes damaged.
            if (healthPercent >= 0.75f)
            {
                score += highHealthBonus;

                DebugLog(
                    $"{player.name}: " +
                    $"High Health Bonus = " +
                    $"{highHealthBonus:F2}");
            }

            // --------------------------------------------------
            // RANGE VALUE
            // --------------------------------------------------

            if (effectiveRange > 0f)
            {
                float rangePercent =
                    Mathf.Clamp01(
                        1f -
                        (distance / effectiveRange));

                float rangeScore =
                    rangePercent *
                    closeRangeBonus;

                score += rangeScore;

                DebugLog(
                    $"{player.name}: " +
                    $"Range Score = " +
                    $"{rangePercent:F2} * " +
                    $"{closeRangeBonus:F2} = " +
                    $"{rangeScore:F2}");
            }

            // --------------------------------------------------
            // AP PENALTY
            // --------------------------------------------------

            float apPenalty =
                statusItem.apCost *
                apCostWeight;

            score -= apPenalty;

            DebugLog(
                $"{player.name}: " +
                $"AP Penalty = -" +
                $"{apPenalty:F2}");

            // --------------------------------------------------
            // TARGET SUMMARY
            // --------------------------------------------------

            DebugLog(
                $"{player.name}: " +
                $"FINAL TARGET SCORE = {score:F2}");

            // --------------------------------------------------
            // BEST TARGET
            // --------------------------------------------------

            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = player;

                DebugLog(
                    $"NEW BEST TARGET: " +
                    $"{player.name}, " +
                    $"Score={score:F2}");
            }
        }

        // --------------------------------------------------
        // TARGET SUMMARY
        // --------------------------------------------------

        DebugLog(
            $"TARGET SUMMARY: " +
            $"ValidTargets={validTargetCount}, " +
            $"Required={minimumTargets}");

        if (validTargetCount < minimumTargets)
        {
            DebugLog(
                $"FAILED: Not enough valid targets. " +
                $"Found={validTargetCount}, " +
                $"Required={minimumTargets}");

            return null;
        }

        if (bestTarget == null)
        {
            DebugLog(
                "FAILED: No best target found.");

            return null;
        }

        // --------------------------------------------------
        // TARGET DATA
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = bestTarget
            };

        // --------------------------------------------------
        // FINAL
        // --------------------------------------------------

        DebugLog(
            $"FINAL NEGATIVE STATUS SCORE = " +
            $"{bestScore:F2}");

        DebugLog(
            $"Returning AIItemEvaluation. " +
            $"Target={bestTarget.name}, " +
            $"Score={bestScore:F2}");

        DebugLog(
            "NEGATIVE STATUS EVALUATION END");

        DebugLog(
            "==================================================");

        return new AIItemEvaluation(
            bestScore,
            targetData);
    }

    private void DebugLog(string message)
    {
        if (!enableDebugLogs)
            return;

        Debug.Log(
            $"[NegativeStatus AI] {message}");
    }
}