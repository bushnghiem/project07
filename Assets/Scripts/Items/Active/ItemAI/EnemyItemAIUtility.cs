using UnityEngine;

public static class EnemyItemAIUtility
{
    public static AIItemCandidate EvaluateItem(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context)
    {
        ActiveItemInstance item =
            enemy.GetActiveItem();

        if (item == null)
        {
            Debug.LogWarning(
                $"{enemy.name} ITEM AI: No active item found.");

            return null;
        }

        ActiveItem itemData =
            item.itemData;

        if (itemData == null)
        {
            Debug.LogWarning(
                $"{enemy.name} ITEM AI: Active item has no itemData.");

            return null;
        }

        if (itemData.aiEvaluator == null)
        {
            Debug.LogWarning(
                $"{enemy.name} ITEM AI: " +
                $"{itemData.name} has NO AI evaluator.");

            return null;
        }

        Debug.Log(
            $"{enemy.name} ITEM AI: Evaluating " +
            $"{itemData.name} using " +
            $"{itemData.aiEvaluator.GetType().Name}");

        AIItemEvaluation evaluation =
            itemData.aiEvaluator.Evaluate(
                enemy,
                item,
                context,
                battleManager);

        if (evaluation == null)
        {
            Debug.LogWarning(
                $"{enemy.name} ITEM AI: " +
                $"{itemData.name} evaluator returned NULL.");

            return null;
        }

        Debug.Log(
            $"{enemy.name} ITEM AI: " +
            $"{itemData.name} score = {evaluation.score}");

        if (evaluation.score == float.MinValue)
        {
            Debug.LogWarning(
                $"{enemy.name} ITEM AI: " +
                $"{itemData.name} returned float.MinValue.");

            return null;
        }

        if (evaluation.targetData == null)
        {
            Debug.LogWarning(
                $"{enemy.name} ITEM AI: " +
                $"{itemData.name} returned NO TARGET.");

            return null;
        }

        Debug.Log(
            $"{enemy.name} ITEM AI: " +
            $"{itemData.name} produced valid target.");

        return new AIItemCandidate(
            item,
            evaluation.targetData,
            evaluation.score);
    }


    public static UnitAction CreateItemAction(
        Enemy enemy,
        AIItemCandidate candidate)
    {
        return new UnitAction
        {
            actor = enemy,
            actionType = ActionType.Item,
            activeItem = candidate.item,
            itemTargetData = candidate.targetData,
            aiScore = candidate.score
        };
    }
}
