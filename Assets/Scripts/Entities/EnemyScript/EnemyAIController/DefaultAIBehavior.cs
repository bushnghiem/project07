using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Default AI")]
public class DefaultAIBehavior : EnemyAIBehavior
{
    [Header("Behavior")]
    public float preferredShootDistancePercent = 0.7f;

    [Header("Personality")]
    [Range(0f, 1f)]
    public float aggression = 0.7f;

    [Range(0f, 1f)]
    public float orbitPreference = 0.5f;

    [Range(0f, 1f)]
    public float decisiveness = 0.7f;

    [Header("Accuracy")]
    [Range(0f, 20f)]
    public float aimErrorAngle = 6f;

    [Header("AP Evaluation")]
    [Tooltip("How much the AI values being able to perform another action after this one.")]
    public float futureActionValue = 1f;

    [Tooltip("How much the AI values saving AP for later.")]
    public float remainingAPValue = 0.15f;

    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context)
    {
        Player target =
            EnemyAIUtility.GetClosestPlayer(
                enemy,
                battleManager);

        if (target == null)
            return null;

        List<UnitAction> candidates = new();

        // --------------------------------------------------
        // SHOOT
        // --------------------------------------------------

        UnitAction shoot =
            new AttackState(aimErrorAngle)
                .DecideAction(enemy, battleManager);

        if (shoot != null && shoot.CanAfford())
        {
            candidates.Add(shoot);
        }

        // --------------------------------------------------
        // MOVE
        // --------------------------------------------------

        UnitAction move =
            new MoveState(preferredShootDistancePercent)
                .DecideAction(enemy, battleManager);

        if (move != null && move.CanAfford())
        {
            candidates.Add(move);
        }

        // --------------------------------------------------
        // ORBIT
        // --------------------------------------------------

        float maxShotRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        UnitAction orbit =
            new OrbitState(
                preferredShootDistancePercent)
                .DecideAction(
                    enemy,
                    battleManager);

        if (orbit != null && orbit.CanAfford())
        {
            candidates.Add(orbit);
        }

        // --------------------------------------------------
        // NO AFFORDABLE ACTION
        // --------------------------------------------------

        if (candidates.Count == 0)
            return null;

        // --------------------------------------------------
        // SCORE ACTIONS
        // --------------------------------------------------

        UnitAction bestAction = null;
        float bestScore = float.MinValue;

        foreach (UnitAction action in candidates)
        {
            float score =
                ScoreAction(
                    action,
                    enemy,
                    target,
                    battleManager);

            Debug.Log(
                $"{enemy.name} evaluating {action.actionType}: " +
                $"Cost={action.APCost}, " +
                $"AP={enemy.CurrentAP}, " +
                $"Score={score:F2}");

            if (score > bestScore)
            {
                bestScore = score;
                bestAction = action;
            }
        }

        // --------------------------------------------------
        // REMEMBER STATE
        // --------------------------------------------------

        if (bestAction != null)
        {
            context.lastActionType =
                bestAction.actionType;

            context.lastActionCost =
                bestAction.APCost;

            context.actionsThisTurn++;
        }

        return bestAction;
    }

    private float ScoreAction(
        UnitAction action,
        Enemy enemy,
        Player target,
        BattleManager battleManager)
    {
        switch (action.actionType)
        {
            case ActionType.Shoot:
                return ScoreShoot(
                    action,
                    enemy,
                    target);

            case ActionType.Move:
                return ScoreMove(
                    action,
                    enemy,
                    target,
                    battleManager);

            default:
                return -1000f;
        }
    }

    private float ScoreShoot(
        UnitAction action,
        Enemy enemy,
        Player target)
    {
        float distance =
            Vector3.Distance(
                enemy.Position,
                target.Position);

        float maxRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        float distance01 =
            Mathf.Clamp01(distance / maxRange);

        bool hasLOS =
            EnemyAIUtility.HasLineOfSight(
                enemy,
                target);

        float score = 0f;

        // Shooting is generally desirable.
        score += aggression * 4f;

        // Strong preference for having LOS.
        if (hasLOS)
            score += 4f;
        else
            score -= 5f;

        // Prefer targets within effective range.
        if (distance <= maxRange)
        {
            score += 2f;
        }
        else
        {
            score -= 4f * (distance01 - 1f);
        }

        // Prefer shooting from closer to the preferred distance.
        float desiredDistance =
            maxRange *
            preferredShootDistancePercent;

        float distanceError =
            Mathf.Abs(distance - desiredDistance);

        float distanceQuality =
            1f -
            Mathf.Clamp01(
                distanceError / maxRange);

        score += distanceQuality * 2f;

        // Expensive attacks should have a little more justification.
        score -= action.APCost * 0.25f;

        // Evaluate what remains after shooting.
        int remainingAP =
            enemy.CurrentAP - action.APCost;

        score += ScoreRemainingAP(
            enemy,
            remainingAP);

        return score;
    }

    private float ScoreMove(
        UnitAction action,
        Enemy enemy,
        Player target,
        BattleManager battleManager)
    {
        float distance =
            Vector3.Distance(
                enemy.Position,
                target.Position);

        float maxShotRange =
            EnemyAIUtility.EstimateShotRange(enemy);

        float desiredDistance =
            maxShotRange *
            preferredShootDistancePercent;

        // Approximate where the move will put the enemy.
        float moveDistance =
            EnemyAIUtility.EstimateMoveRange(enemy) *
            action.powerPercent;

        Vector3 futurePosition =
            enemy.Position +
            action.direction.normalized *
            moveDistance;

        float futureDistance =
            Vector3.Distance(
                futurePosition,
                target.Position);

        float currentError =
            Mathf.Abs(
                distance -
                desiredDistance);

        float futureError =
            Mathf.Abs(
                futureDistance -
                desiredDistance);

        float improvement =
            currentError -
            futureError;

        float score = 0f;

        // Reward moving toward desired range.
        score += improvement * 0.15f;

        // Reward movement if it creates LOS.
        bool currentLOS =
            EnemyAIUtility.HasLineOfSight(
                enemy,
                target);

        // We can't directly ask LOS from the hypothetical
        // future position without temporarily moving the ship,
        // so use the existing MoveState's evaluation indirectly.

        if (!currentLOS)
            score += 2f;

        // If already at desired range, movement is less valuable.
        if (currentError < maxShotRange * 0.1f)
            score -= 2f;

        // Orbiting preference.
        score += orbitPreference;

        // Expensive movement should provide meaningful value.
        score -= action.APCost * 0.15f;

        // Evaluate remaining AP.
        int remainingAP =
            enemy.CurrentAP - action.APCost;

        score += ScoreRemainingAP(
            enemy,
            remainingAP);

        return score;
    }

    private float ScoreRemainingAP(
        Enemy enemy,
        int remainingAP)
    {
        if (remainingAP <= 0)
            return 0f;

        float score = 0f;

        // Can we shoot after this action?
        if (remainingAP >= enemy.GetShootCost())
        {
            score += futureActionValue * aggression;
        }

        // Can we move again?
        if (remainingAP >= enemy.GetMoveCost())
        {
            score += futureActionValue * 0.5f;
        }

        // Small value for unused AP.
        score += remainingAP * remainingAPValue;

        return score;
    }
}