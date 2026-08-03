using UnityEngine;

[System.Serializable]
public class QuestInstance
{
    public QuestData quest;

    public bool completed;

    public int targetFloor;

    public Vector2Int targetPosition;
}
