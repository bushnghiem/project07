using UnityEngine;

public enum TileType
{
    Empty,
    Wall,
    Combat,
    Event,
    Portal
}

[System.Serializable]
public class TileData
{
    public TileType tileType;
    public bool IsWalkable => tileType != TileType.Wall;
}
