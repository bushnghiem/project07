using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Items/Self Destruct Evaluator")]
public class SelfDestructItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    public float enemyValue = 10f;
    public float damagePotentialWeight = 5f;
    public float multiTargetBonus = 5f;
    public float selfPreservationWeight = 10f;
    public float lethalBonus = 20f;

    [Header("Requirements")]
    public int minimumTargets = 1;

    [Range(0f, 1f)]
    public float minimumHealthPercent = 0.5f;

    [Header("Explosion")]
    public float explosionRadius = 3f;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    public override AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager)
    {
        DebugLog(
            "==================================================");

        DebugLog(
            $"SELF-DESTRUCT EVALUATION START: " +
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

        SelfDestructionItem selfDestruct =
            item.itemData as SelfDestructionItem;

        if (selfDestruct == null)
        {
            DebugLog(
                "FAILED: item is not a SelfDestructionItem");

            return null;
        }

        DebugLog(
            $"SelfDestruct item confirmed. " +
            $"Explosion={selfDestruct.explosion}");

        // --------------------------------------------------
        // CAN USE
        // --------------------------------------------------

        DebugLog(
            $"Checking CanUse: " +
            $"Cooldown={item.GetRemainingCooldown()}, " +
            $"Charges={enemy.CurrentCharges}/" +
            $"{selfDestruct.chargeCost}, " +
            $"AP={enemy.CurrentAP}/" +
            $"{selfDestruct.apCost}");

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

        DebugLog(
            $"Self Health: " +
            $"{enemy.CurrentHealth:F1}/" +
            $"{enemy.MaxHealth:F1} " +
            $"({healthPercent:P1})");

        DebugLog(
            $"Minimum Health Percent Required: " +
            $"{minimumHealthPercent:P1}");

        if (healthPercent > minimumHealthPercent)
        {
            DebugLog(
                $"FAILED: Enemy is too healthy to self-destruct. " +
                $"Health={healthPercent:P1}");

            return null;
        }

        DebugLog(
            "Health requirement PASSED.");

        // --------------------------------------------------
        // GET PLAYERS
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
        // EXPLOSION RADIUS
        // --------------------------------------------------

        DebugLog(
            $"Explosion Radius = {explosionRadius}");

        // --------------------------------------------------
        // FIND TARGETS
        // --------------------------------------------------

        int targetCount = 0;

        float totalTargetHealth = 0f;
        float totalTargetMissingHealth = 0f;

        foreach (Player player in battleManager.allPlayers)
        {
            if (player == null)
            {
                DebugLog(
                    "Skipping player: null");

                continue;
            }

            DebugLog(
                $"Checking player: {player.name}");

            if (player.isDead)
            {
                DebugLog(
                    $"Skipping {player.name}: DEAD");

                continue;
            }

            float distance =
                Vector3.Distance(
                    enemy.Position,
                    player.Position);

            DebugLog(
                $"{player.name}: " +
                $"Distance={distance:F2}, " +
                $"ExplosionRadius={explosionRadius:F2}");

            if (distance > explosionRadius)
            {
                DebugLog(
                    $"Skipping {player.name}: " +
                    $"OUTSIDE explosion radius");

                continue;
            }

            DebugLog(
                $"TARGET HIT: {player.name}");

            targetCount++;

            if (player.MaxHealth > 0f)
            {
                float playerHealthPercent =
                    player.CurrentHealth /
                    player.MaxHealth;

                float missingHealth =
                    1f -
                    playerHealthPercent;

                totalTargetHealth +=
                    player.CurrentHealth;

                totalTargetMissingHealth +=
                    missingHealth;

                DebugLog(
                    $"{player.name}: " +
                    $"Health={player.CurrentHealth:F1}/" +
                    $"{player.MaxHealth:F1}, " +
                    $"Missing={missingHealth:P1}");
            }
        }

        // --------------------------------------------------
        // TARGET SUMMARY
        // --------------------------------------------------

        DebugLog(
            $"TARGET SUMMARY: " +
            $"Count={targetCount}, " +
            $"Required={minimumTargets}, " +
            $"TotalMissingHealth={totalTargetMissingHealth:F2}");

        if (targetCount < minimumTargets)
        {
            DebugLog(
                $"FAILED: Not enough targets. " +
                $"Found={targetCount}, " +
                $"Required={minimumTargets}");

            return null;
        }

        DebugLog(
            "Target requirement PASSED.");

        // --------------------------------------------------
        // SCORE
        // --------------------------------------------------

        float score = 0f;

        float targetScore =
            targetCount *
            enemyValue;

        score += targetScore;

        DebugLog(
            $"Target Score: " +
            $"{targetCount} * {enemyValue} = " +
            $"{targetScore:F2}");

        // --------------------------------------------------
        // MULTI TARGET BONUS
        // --------------------------------------------------

        if (targetCount > 1)
        {
            float multiTargetScore =
                (targetCount - 1) *
                multiTargetBonus;

            score += multiTargetScore;

            DebugLog(
                $"Multi Target Bonus: " +
                $"({targetCount} - 1) * " +
                $"{multiTargetBonus} = " +
                $"{multiTargetScore:F2}");
        }
        else
        {
            DebugLog(
                "No multi-target bonus.");
        }

        // --------------------------------------------------
        // DAMAGE POTENTIAL
        // --------------------------------------------------

        float damagePotentialScore =
            totalTargetMissingHealth *
            damagePotentialWeight;

        score += damagePotentialScore;

        DebugLog(
            $"Damage Potential Score: " +
            $"{totalTargetMissingHealth:F2} * " +
            $"{damagePotentialWeight} = " +
            $"{damagePotentialScore:F2}");

        // --------------------------------------------------
        // SELF PRESERVATION
        // --------------------------------------------------

        float missingSelfHealth =
            1f -
            healthPercent;

        float selfPreservationScore =
            missingSelfHealth *
            selfPreservationWeight;

        score += selfPreservationScore;

        DebugLog(
            $"Self Preservation Score: " +
            $"{missingSelfHealth:P1} * " +
            $"{selfPreservationWeight} = " +
            $"{selfPreservationScore:F2}");

        // --------------------------------------------------
        // LETHAL BONUS
        // --------------------------------------------------

        if (healthPercent <= 0.25f)
        {
            score += lethalBonus;

            DebugLog(
                $"Lethal Bonus: +{lethalBonus:F2}");
        }
        else
        {
            DebugLog(
                $"No Lethal Bonus. " +
                $"Health={healthPercent:P1}");
        }

        // --------------------------------------------------
        // AP PENALTY
        // --------------------------------------------------

        float apPenalty =
            selfDestruct.apCost *
            0.5f;

        score -= apPenalty;

        DebugLog(
            $"AP Penalty: -{apPenalty:F2}");

        // --------------------------------------------------
        // FINAL SCORE
        // --------------------------------------------------

        DebugLog(
            $"FINAL SELF-DESTRUCT SCORE = {score:F2}");

        // --------------------------------------------------
        // TARGET DATA
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
            "SELF-DESTRUCT EVALUATION END");

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
            $"[SelfDestruct AI] {message}");
    }
}
