using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FloorData
{
    public int floorIndex;
    public int floorSeed;

    public FloorContentProfile contentProfile;

    public Vector2Int currentGridPosition;

    public EncounterData currentEncounter;

    public int timeElapsed = 0;
    public int nextCorruptionTimeThreshold = 5;

    public List<Vector2Int> clearedCombatTiles = new();
    public List<Vector2Int> clearedEventTiles = new();
    public Dictionary<Vector2Int, List<int>> usedEventOptions = new();

    public List<ShopData> shops = new List<ShopData>();

    public int corruptionRadius = -1;

    public bool hasInitializedSpawn = false;
}
