using UnityEngine;

public enum QuestEventType
{
    TileCorrupted,
    PlayerDied,
    ItemLost,
    FloorExited
}

public struct QuestEvent
{
    public QuestEventType type;

    public Vector2Int position;

    public string playerId;

    public Item item;

    public int floor;

    public QuestEvent(
        QuestEventType type,
        Vector2Int position = default,
        string playerId = null,
        Item item = null,
        int floor = -1)
    {
        this.type = type;
        this.position = position;
        this.playerId = playerId;
        this.item = item;
        this.floor = floor;
    }
}
