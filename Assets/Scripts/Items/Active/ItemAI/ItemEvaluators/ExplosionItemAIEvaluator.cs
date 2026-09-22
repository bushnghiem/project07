using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Items/Explosion Evaluator")]
public class ExplosionItemAIEvaluator : ActiveItemAIEvaluator
{
    [Header("Scoring")]
    public float playerHitWeight = 5f;
    public float multipleTargetBonus = 4f;

    [Header("Explosion")]
    public float explosionRadius = 5f;

    public override AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager)
    {
        Debug.Log(
            $"{enemy.name} EXPLOSION EVALUATOR STARTED");

        // --------------------------------------------------
        // BASIC VALIDATION
        // --------------------------------------------------

        if (item == null)
        {
            Debug.LogWarning(
                $"{enemy.name} EXPLOSION: Item is null.");

            return null;
        }

        if (!item.CanUse(enemy))
        {
            Debug.LogWarning(
                $"{enemy.name} EXPLOSION: Cannot use item.");

            return null;
        }

        if (battleManager == null)
        {
            Debug.LogWarning(
                $"{enemy.name} EXPLOSION: BattleManager is null.");

            return null;
        }

        // --------------------------------------------------
        // GET PLAYERS
        // --------------------------------------------------

        List<Unit> players =
            battleManager.allPlayers;

        Debug.Log(
            $"{enemy.name} EXPLOSION: Found " +
            $"{(players == null ? 0 : players.Count)} players.");

        if (players == null || players.Count == 0)
            return null;

        // --------------------------------------------------
        // FIND BEST EXPLOSION TARGET
        // --------------------------------------------------

        Player bestTarget = null;
        float bestScore = float.MinValue;

        foreach (Unit unit in players)
        {
            if (unit == null)
                continue;

            Player player = unit as Player;

            if (player == null)
                continue;

            if (player.isDead)
                continue;

            float distance =
                Vector3.Distance(
                    enemy.Position,
                    player.Position);

            Debug.Log(
                $"{enemy.name} EXPLOSION: Checking " +
                $"{player.name}, " +
                $"distance={distance:F2}, " +
                $"range={item.itemData.range}");

            // Outside item's targeting range.
            if (distance > item.itemData.range)
            {
                Debug.Log(
                    $"{enemy.name} EXPLOSION: " +
                    $"{player.name} is outside range.");

                continue;
            }

            float score =
                EvaluateTarget(
                    player.transform.position,
                    item);

            Debug.Log(
                $"{enemy.name} EXPLOSION: " +
                $"{player.name} score={score:F2}");

            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = player;
            }
        }

        // --------------------------------------------------
        // NO VALID TARGET
        // --------------------------------------------------

        if (bestTarget == null)
        {
            Debug.Log(
                $"{enemy.name} EXPLOSION: " +
                $"No valid target found.");

            return null;
        }

        Debug.Log(
            $"{enemy.name} EXPLOSION: BEST TARGET = " +
            $"{bestTarget.name}, " +
            $"Score={bestScore:F2}");

        // --------------------------------------------------
        // RETURN SCORE + TARGET
        // --------------------------------------------------

        ItemTargetData targetData =
            new ItemTargetData
            {
                targetUnit = bestTarget
            };

        return new AIItemEvaluation(
            bestScore,
            targetData);
    }

    private float EvaluateTarget(
        Vector3 position,
        ActiveItemInstance item)
    {
        Collider[] hits =
            Physics.OverlapSphere(
                position,
                explosionRadius);

        HashSet<Player> playersHit =
            new HashSet<Player>();

        foreach (Collider hit in hits)
        {
            Player player =
                hit.GetComponentInParent<Player>();

            if (player != null &&
                !player.isDead)
            {
                playersHit.Add(player);
            }
        }

        int playerCount = playersHit.Count;


        if (playerCount == 0)
            return float.MinValue;

        float score =
            playerCount *
            playerHitWeight;

        if (playerCount > 1)
        {
            score +=
                (playerCount - 1) *
                multipleTargetBonus;
        }

        // AP cost penalty.
        score -=
            item.itemData.apCost *
            0.5f;

        return score;
    }
}
