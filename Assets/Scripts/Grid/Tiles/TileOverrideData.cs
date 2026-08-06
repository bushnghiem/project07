using UnityEngine;

[System.Serializable]
public class TileOverrideData
{
    public bool hasTileTypeOverride;
    public TileType tileType;

    public bool hasEncounterOverride;
    public EncounterData encounter;

    public bool hasEventOverride;
    public EventData assignedEvent;

    public bool isElite;
    public bool isCorrupted;
}