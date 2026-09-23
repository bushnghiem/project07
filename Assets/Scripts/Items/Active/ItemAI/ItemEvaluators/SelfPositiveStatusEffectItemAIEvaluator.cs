using UnityEngine;

[CreateAssetMenu(menuName = "AI/Items/Self Positive Status Evaluator")]
public class SelfPositiveStatusEffectAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    [Tooltip("Base value of applying a positive status.")]
    public float statusValue = 10f;

    [Tooltip("Additional value per stack.")]
    public float stackValue = 3f;

    [Tooltip("Additional value when the AI is low on health.")]
    public float lowHealthBonus = 5f;

    [Tooltip("Additional value when the AI is missing shield.")]
    public float lowShieldBonus = 3f;

    [Header("Requirements")]
    public bool requireStatusEffect = true;

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
            $"SELF POSITIVE STATUS EVALUATION START: " +
            $"Enemy={enemy?.name}, " +
            $"Item={item?.itemData?.name}");

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

        SelfStatusEffectItem statusItem =
            item.itemData as SelfStatusEffectItem;

        if (statusItem == null)
        {
            DebugLog(
                "FAILED: item is not a SelfStatusEffectItem");

            return null;
        }

        if (statusItem.statusEffect == null && requireStatusEffect)
        {
            DebugLog(
                "FAILED: statusEffect == null");

            return null;
        }

        if (statusItem.stacks <= 0)
        {
            DebugLog(
                "FAILED: stacks <= 0");

            return null;
        }

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

        float healthPercent = 1f;

        if (enemy.MaxHealth > 0f)
        {
            healthPercent =
                Mathf.Clamp01(
                    enemy.CurrentHealth /
                    enemy.MaxHealth);
        }

        float missingHealth =
            1f - healthPercent;

        // --------------------------------------------------
        // SCORE
        // --------------------------------------------------

        float score = statusValue;

        float stackScore =
            statusItem.stacks *
            stackValue;

        score += stackScore;

        DebugLog(
            $"Base Status Score={statusValue:F2}");

        DebugLog(
            $"Stack Score={stackScore:F2}");

        // A positive defensive/buff status becomes more valuable
        // when the enemy is in danger.
        float healthScore =
            missingHealth *
            lowHealthBonus;

        score += healthScore;

        DebugLog(
            $"Low Health Score=" +
            $"{healthScore:F2}");

        // --------------------------------------------------
        // AP PENALTY
        // --------------------------------------------------

        float apPenalty =
            statusItem.apCost *
            0.5f;

        score -= apPenalty;

        DebugLog(
            $"AP Penalty=-{apPenalty:F2}");

        // --------------------------------------------------
        // TARGET SELF
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = enemy
            };

        DebugLog(
            $"FINAL SCORE={score:F2}");

        DebugLog(
            $"TARGET={enemy.name}");

        DebugLog("SELF POSITIVE STATUS EVALUATION END");
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
            $"[SelfPositiveStatus AI] {message}");
    }

}