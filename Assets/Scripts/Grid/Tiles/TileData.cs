using UnityEngine;

public enum TileType
{
    Empty,
    Wall,
    Combat,
    Event,
    Shop,
    Portal
}

[System.Serializable]
public class TileData
{
    public TileType tileType;
    public bool IsWalkable => tileType != TileType.Wall;

    public EncounterData assignedEncounter;
}
