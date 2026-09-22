using UnityEngine;
using System.Collections.Generic;

public enum FieldTargetPreference
{
    Enemies,
    Allies
}

[CreateAssetMenu(menuName = "AI/Items/Field Placement Evaluator")]
public class FieldPlacementItemAIEvaluator
    : ActiveItemAIEvaluator
{
    [Header("Field")]
    public FieldTargetPreference targetPreference =
        FieldTargetPreference.Enemies;

    [Tooltip("Radius of the field's effect.")]
    public float fieldRadius = 3f;

    [Header("Scoring")]
    [Tooltip("Score per desirable unit inside the field.")]
    public float targetValue = 10f;

    [Tooltip("Additional score based on how much health the target has.")]
    public float healthWeight = 5f;

    [Tooltip("Bonus for hitting multiple targets.")]
    public float multiTargetBonus = 5f;

    [Tooltip("Penalty for placing the field very far away.")]
    public float distancePenalty = 0.25f;

    [Header("Position Search")]
    [Tooltip("Number of candidate positions around each target.")]
    public int samplesPerTarget = 8;

    [Tooltip("Distance from the target at which candidate positions are tested.")]
    public float sampleOffset = 2f;

    [Tooltip("Also consider placing the field directly on targets.")]
    public bool testTargetPositions = true;

    public override AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager)
    {
        if (enemy == null ||
            item == null ||
            battleManager == null)
        {
            return null;
        }

        if (!item.CanUse(enemy))
            return null;

        if (item.itemData == null)
            return null;

        // --------------------------------------------------
        // FIND CANDIDATE TARGETS
        // --------------------------------------------------

        List<Unit> candidates =
            GetCandidateUnits(
                enemy,
                battleManager);

        if (candidates == null ||
            candidates.Count == 0)
        {
            return null;
        }

        // --------------------------------------------------
        // FIND BEST POSITION
        // --------------------------------------------------

        Vector3 bestPosition = Vector3.zero;
        float bestScore = float.MinValue;

        foreach (Unit target in candidates)
        {
            if (target == null)
                continue;

            EvaluatePosition(
                enemy,
                item,
                target.Position,
                candidates,
                ref bestPosition,
                ref bestScore);

            // --------------------------------------------------
            // TEST POSITIONS AROUND TARGET
            // --------------------------------------------------

            if (samplesPerTarget <= 0)
                continue;

            for (int i = 0;
                 i < samplesPerTarget;
                 i++)
            {
                float angle =
                    (360f / samplesPerTarget) * i;

                Vector3 direction =
                    Quaternion.Euler(
                        0f,
                        angle,
                        0f) *
                    Vector3.forward;

                Vector3 position =
                    target.Position +
                    direction *
                    sampleOffset;

                position.y =
                    enemy.Position.y;

                EvaluatePosition(
                    enemy,
                    item,
                    position,
                    candidates,
                    ref bestPosition,
                    ref bestScore);
            }
        }

        // --------------------------------------------------
        // NO VALID POSITION
        // --------------------------------------------------

        if (bestScore == float.MinValue)
            return null;

        Debug.Log(
            $"{enemy.name} FIELD AI: " +
            $"BestPosition={bestPosition}, " +
            $"Score={bestScore:F2}");

        return new AIItemEvaluation(
            bestScore,
            new ItemTargetData
            {
                targetPosition = bestPosition
            });
    }

    // ======================================================
    // CANDIDATE UNITS
    // ======================================================

    private List<Unit> GetCandidateUnits(
        Enemy enemy,
        BattleManager battleManager)
    {
        List<Unit> result =
            new List<Unit>();

        if (targetPreference ==
            FieldTargetPreference.Enemies)
        {
            if (battleManager.allPlayers == null)
                return result;

            foreach (Player player
                in battleManager.allPlayers)
            {
                if (player == null ||
                    player.isDead)
                {
                    continue;
                }

                result.Add(player);
            }
        }
        else
        {
            if (battleManager.allEnemies == null)
                return result;

            foreach (Unit unit
                in battleManager.allEnemies)
            {
                if (unit == null)
                    continue;

                Enemy ally =
                    unit as Enemy;

                if (ally == null ||
                    ally.isDead)
                {
                    continue;
                }

                result.Add(ally);
            }
        }

        return result;
    }

    // ======================================================
    // POSITION EVALUATION
    // ======================================================

    private void EvaluatePosition(
        Enemy enemy,
        ActiveItemInstance item,
        Vector3 position,
        List<Unit> candidates,
        ref Vector3 bestPosition,
        ref float bestScore)
    {
        // --------------------------------------------------
        // RANGE
        // --------------------------------------------------

        float distance =
            Vector3.Distance(
                enemy.Position,
                position);

        if (distance > item.itemData.range)
            return;

        // --------------------------------------------------
        // VALID POSITION
        // --------------------------------------------------

        if (!IsValidFieldPosition(
                enemy,
                item,
                position))
        {
            return;
        }

        // --------------------------------------------------
        // SCORE UNITS INSIDE FIELD
        // --------------------------------------------------

        float score = 0f;
        int targetCount = 0;

        foreach (Unit unit in candidates)
        {
            if (unit == null)
                continue;

            UnitBase unitBase = unit as UnitBase;

            float targetDistance =
                Vector3.Distance(
                    position,
                    unitBase.Position);

            if (targetDistance >
                fieldRadius)
            {
                continue;
            }

            targetCount++;

            score += targetValue;

            // --------------------------------------------------
            // HEALTH VALUE
            // --------------------------------------------------

            if (unitBase.MaxHealth > 0f)
            {
                float healthPercent =
                    unitBase.CurrentHealth /
                    unitBase.MaxHealth;

                if (targetPreference ==
                    FieldTargetPreference.Enemies)
                {
                    /*
                     * Damaging wounded enemies is slightly
                     * less valuable than damaging healthy ones
                     * if your field damage is fixed.
                     *
                     * This can be inverted if the field is
                     * intended as an execution tool.
                     */
                    score +=
                        healthPercent *
                        healthWeight;
                }
                else
                {
                    /*
                     * Healing injured allies is more valuable.
                     */
                    float missingHealth =
                        1f -
                        healthPercent;

                    score +=
                        missingHealth *
                        healthWeight;
                }
            }
        }

        // --------------------------------------------------
        // REQUIRE AT LEAST ONE TARGET
        // --------------------------------------------------

        if (targetCount == 0)
            return;

        // --------------------------------------------------
        // MULTI TARGET BONUS
        // --------------------------------------------------

        if (targetCount > 1)
        {
            score +=
                (targetCount - 1) *
                multiTargetBonus;
        }

        // --------------------------------------------------
        // DISTANCE PENALTY
        // --------------------------------------------------

        score -=
            distance *
            distancePenalty;

        // --------------------------------------------------
        // AP PENALTY
        // --------------------------------------------------

        score -=
            item.itemData.apCost *
            0.5f;

        // --------------------------------------------------
        // BEST POSITION
        // --------------------------------------------------

        if (score > bestScore)
        {
            bestScore = score;
            bestPosition = position;
        }
    }

    // ======================================================
    // POSITION VALIDATION
    // ======================================================

    private bool IsValidFieldPosition(
        Enemy enemy,
        ActiveItemInstance item,
        Vector3 position)
    {
        /*
         * This should eventually use the item's own
         * IsValidTarget implementation.
         */

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetPosition = position
            };

        return item.itemData.IsValidTarget(
            enemy,
            targetData);
    }
}

