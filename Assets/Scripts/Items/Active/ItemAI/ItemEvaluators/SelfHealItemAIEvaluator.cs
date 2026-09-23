using UnityEngine;

[CreateAssetMenu(menuName = "AI/Items/Self Heal Evaluator")]
public class SelfHealItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    [Tooltip("How valuable each percentage of missing health is.")]
    public float missingHealthWeight = 10f;

    [Tooltip("Additional value when the AI is critically injured.")]
    public float criticalHealthBonus = 20f;

    [Tooltip("Health percentage considered critical.")]
    [Range(0f, 1f)]
    public float criticalHealthPercent = 0.25f;

    [Header("Requirements")]
    [Tooltip("AI will not use the item if it has less missing health than this.")]
    [Range(0f, 1f)]
    public float minimumMissingHealth = 0.2f;

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
            $"SELF HEAL EVALUATION START: " +
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

        // --------------------------------------------------
        // ITEM TYPE
        // --------------------------------------------------

        SelfHealItem healItem =
            item.itemData as SelfHealItem;

        if (healItem == null)
        {
            DebugLog(
                "FAILED: item is not a SelfHealItem");
            return null;
        }

        DebugLog(
            $"Self Heal confirmed. " +
            $"Heal Amount={healItem.healAmount}");

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
        // HEALTH
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
            $"Missing Health = {missingHealth:P1}");

        // --------------------------------------------------
        // REQUIREMENT
        // --------------------------------------------------

        if (missingHealth < minimumMissingHealth)
        {
            DebugLog(
                $"FAILED: Not enough missing health. " +
                $"Missing={missingHealth:P1}, " +
                $"Required={minimumMissingHealth:P1}");

            return null;
        }

        DebugLog("Missing health requirement PASSED.");

        // --------------------------------------------------
        // SCORE
        // --------------------------------------------------

        float score =
            missingHealth *
            missingHealthWeight;

        DebugLog(
            $"Missing Health Score: " +
            $"{missingHealth:F2} * " +
            $"{missingHealthWeight:F2} = " +
            $"{score:F2}");

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
        // AP PENALTY
        // --------------------------------------------------

        float apPenalty =
            healItem.apCost *
            apPenaltyMultiplier;

        score -= apPenalty;

        DebugLog(
            $"AP Penalty: -{apPenalty:F2}");

        // --------------------------------------------------
        // TARGET SELF
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = enemy
            };

        DebugLog(
            $"FINAL SELF HEAL SCORE = {score:F2}");

        DebugLog(
            $"Returning evaluation. " +
            $"Target={enemy.name}");

        DebugLog("SELF HEAL EVALUATION END");
        DebugLog("==================================================");

        return new AIItemEvaluation(
            score,
            targetData);
    }

    private void DebugLog(string message)
    {
        if (!enableDebugLogs)
            return;

        Debug.Log(
            $"[SelfHeal AI] {message}");
    }
}
