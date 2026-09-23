using UnityEngine;

[CreateAssetMenu(menuName = "AI/Items/Negative Self Status Effect Evaluator")]
public class NegativeSelfStatusEffectItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    [Tooltip("Base value for applying the status to yourself.")]
    public float statusValue = 5f;

    [Tooltip("Additional value when the enemy is low health.")]
    public float lowHealthBonus = 5f;

    [Tooltip("Additional value when the enemy is high health.")]
    public float highHealthBonus = 0f;

    [Tooltip("Extra value when the enemy is critically low health.")]
    public float criticalHealthBonus = 10f;

    [Header("Health Requirements")]
    [Tooltip("Minimum health percentage required before using the item.")]
    [Range(0f, 1f)]
    public float minimumHealthPercent = 0f;

    [Tooltip("If enabled, the AI can use this item even when at low health.")]
    public bool allowLowHealthUse = true;

    [Range(0f, 1f)]
    public float criticalHealthPercent = 0.25f;

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
            $"NEGATIVE SELF STATUS EVALUATION START: " +
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

        DebugLog(
            $"Item Data = {item.itemData.name}");

        // --------------------------------------------------
        // ITEM TYPE
        // --------------------------------------------------

        SelfStatusEffectItem statusItem =
            item.itemData as SelfStatusEffectItem;

        if (statusItem == null)
        {
            DebugLog(
                "FAILED: item is not a " +
                "SelfStatusEffectItem");

            return null;
        }

        DebugLog(
            $"Self status item confirmed. " +
            $"Effect={statusItem.statusEffect}, " +
            $"Stacks={statusItem.stacks}");

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

        DebugLog(
            "Status effect validation PASSED.");

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
        // HEALTH VALIDATION
        // --------------------------------------------------

        if (enemy.MaxHealth <= 0f)
        {
            DebugLog(
                $"FAILED: MaxHealth <= 0. " +
                $"Current={enemy.CurrentHealth}, " +
                $"Max={enemy.MaxHealth}");

            return null;
        }

        float healthPercent =
            enemy.CurrentHealth /
            enemy.MaxHealth;

        float missingHealth =
            1f - healthPercent;

        DebugLog(
            $"Self Health: " +
            $"{enemy.CurrentHealth:F1}/" +
            $"{enemy.MaxHealth:F1} " +
            $"({healthPercent:P1})");

        DebugLog(
            $"Minimum Health Percent Required: " +
            $"{minimumHealthPercent:P1}");

        // --------------------------------------------------
        // HEALTH REQUIREMENT
        // --------------------------------------------------

        if (healthPercent < minimumHealthPercent)
        {
            DebugLog(
                $"FAILED: Enemy health is below " +
                $"minimum allowed percentage. " +
                $"Health={healthPercent:P1}");

            return null;
        }

        // --------------------------------------------------
        // LOW HEALTH CHECK
        // --------------------------------------------------

        if (!allowLowHealthUse &&
            healthPercent <= criticalHealthPercent)
        {
            DebugLog(
                $"FAILED: Low health use disabled. " +
                $"Health={healthPercent:P1}, " +
                $"Critical={criticalHealthPercent:P1}");

            return null;
        }

        // --------------------------------------------------
        // SCORE
        // --------------------------------------------------

        float score = 0f;

        // Base value.
        score += statusValue;

        DebugLog(
            $"Base Status Value: " +
            $"+{statusValue:F2}");

        // --------------------------------------------------
        // LOW HEALTH BONUS
        // --------------------------------------------------

        if (healthPercent <= 0.5f)
        {
            float lowHealthScore =
                missingHealth *
                lowHealthBonus;

            score += lowHealthScore;

            DebugLog(
                $"Low Health Score: " +
                $"{missingHealth:F2} * " +
                $"{lowHealthBonus:F2} = " +
                $"{lowHealthScore:F2}");
        }
        else
        {
            DebugLog(
                "No low-health bonus.");
        }

        // --------------------------------------------------
        // CRITICAL HEALTH BONUS
        // --------------------------------------------------

        if (healthPercent <= criticalHealthPercent)
        {
            score += criticalHealthBonus;

            DebugLog(
                $"Critical Health Bonus: " +
                $"+{criticalHealthBonus:F2}");
        }
        else
        {
            DebugLog(
                $"No Critical Health Bonus. " +
                $"Health={healthPercent:P1}");
        }

        // --------------------------------------------------
        // HIGH HEALTH BONUS
        // --------------------------------------------------

        if (healthPercent >= 0.75f)
        {
            score += highHealthBonus;

            DebugLog(
                $"High Health Bonus: " +
                $"+{highHealthBonus:F2}");
        }

        // --------------------------------------------------
        // AP PENALTY
        // --------------------------------------------------

        float apPenalty =
            statusItem.apCost *
            apCostWeight;

        score -= apPenalty;

        DebugLog(
            $"AP Penalty: -" +
            $"{apPenalty:F2}");

        // --------------------------------------------------
        // FINAL SCORE
        // --------------------------------------------------

        DebugLog(
            $"FINAL NEGATIVE SELF STATUS SCORE = " +
            $"{score:F2}");

        // --------------------------------------------------
        // TARGET SELF
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = enemy
            };

        DebugLog(
            $"Returning AIItemEvaluation. " +
            $"Target={enemy.name}, " +
            $"Score={score:F2}");

        DebugLog(
            "NEGATIVE SELF STATUS EVALUATION END");

        DebugLog(
            "==================================================");

        return new AIItemEvaluation(
            score,
            targetData);
    }

    private void DebugLog(string message)
    {
        if (!enableDebugLogs)
            return;

        Debug.Log(
            $"[NegativeSelfStatus AI] {message}");
    }
}