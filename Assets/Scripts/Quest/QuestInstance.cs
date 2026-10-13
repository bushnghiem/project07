using UnityEngine;

public enum QuestState
{
    Active,
    Completed,
    Failed
}

[System.Serializable]
public class QuestInstance
{
    public QuestData quest;

    public QuestState state = QuestState.Active;

    public int targetFloor;

    public Vector2Int targetPosition;
    public bool hasTargetPosition;

    public object runtimeData;

    public string deliveryPlayerId;

    public string failureReason;

    public bool IsActive => state == QuestState.Active;
    public bool IsCompleted => state == QuestState.Completed;
    public bool IsFailed => state == QuestState.Failed;
    public bool IsFinished => state != QuestState.Active;
}