using UnityEngine;

public abstract class ActiveItemAIEvaluator : ScriptableObject
{
    public abstract AIItemEvaluation Evaluate(
        Enemy enemy,
        ActiveItemInstance item,
        EnemyAIContext context,
        BattleManager battleManager
    );
}


