using UnityEngine;

[CreateAssetMenu(menuName = "AI/Items/Self Shield Evaluator")]
public class SelfShieldItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    public float shieldValue = 5f;
    public float lowHealthBonus = 5f;

    [Header("AI Perception")]
    [Tooltip("The amount of shield the AI considers highly valuable.")]
    public float desiredShield = 3f;

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

        if (item.itemData == null)
            return null;

        ShieldItem shieldItem =
            item.itemData as ShieldItem;

        if (shieldItem == null)
            return null;

        int currentShield =
            enemy.CurrentShield;

        int shieldAdded =
            shieldItem.shieldAmount;

        if (shieldAdded <= 0)
            return null;

        // --------------------------------------------------
        // SHIELD NEED
        // --------------------------------------------------

        float shieldNeed =
            Mathf.Clamp01(
                1f -
                (float)currentShield /
                desiredShield);

        // Don't bother adding more shield if we already
        // have enough protection.
        if (shieldNeed <= 0f)
            return null;

        // --------------------------------------------------
        // HEALTH
        // --------------------------------------------------

        float healthPercent =
            enemy.CurrentHealth /
            enemy.MaxHealth;

        float missingHealth =
            1f - healthPercent;

        // --------------------------------------------------
        // SCORE
        // --------------------------------------------------

        float score = 0f;

        // Shield is valuable because each point represents
        // another hit the enemy can absorb.
        score +=
            shieldNeed *
            shieldAdded *
            shieldValue;

        // Being low on health makes protection more valuable.
        score +=
            missingHealth *
            lowHealthBonus;

        // --------------------------------------------------
        // AP COST
        // --------------------------------------------------

        score -=
            item.itemData.apCost *
            0.5f;

        // --------------------------------------------------
        // TARGET SELF
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = enemy
            };

        return new AIItemEvaluation(
            score,
            targetData);
    }
}
