using UnityEngine;

public abstract class EnemyAIBehavior : ScriptableObject
{
    [SerializeField] private string behaviorID;

    public string BehaviorID => behaviorID;

    // Optional hook called at the start of an enemy's turn.
    // Individual AI behaviors can override this if needed.
    public virtual void OnTurnStarted(
        Enemy enemy,
        EnemyAIContext context)
    {
    }

    public abstract UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager,
        EnemyAIContext context
    );
}