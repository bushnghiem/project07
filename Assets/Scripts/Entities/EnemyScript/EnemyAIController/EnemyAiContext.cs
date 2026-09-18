using UnityEngine;

[System.Serializable]
public class EnemyAIContext
{
    public EnemyState lastState;

    public float aggressionMemory;

    public int turnsWithoutLOS;

    public Vector3 lastKnownTargetPosition;

    public ActionType lastActionType;

    public int actionsThisTurn;

    public int lastActionCost;
}