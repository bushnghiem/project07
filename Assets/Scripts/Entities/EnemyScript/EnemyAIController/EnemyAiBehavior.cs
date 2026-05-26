using UnityEngine;

public abstract class EnemyAIBehavior : ScriptableObject
{
    [SerializeField] private string behaviorID;

    public string BehaviorID => behaviorID;

    public abstract UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context
    );
}