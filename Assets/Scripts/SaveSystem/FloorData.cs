using UnityEngine;
using System.Collections.Generic;

public enum PendingRewardType
{
    None,
    Elite,
    Boss
}

[System.Serializable]
public class FloorData
{
    public int floorIndex;
    public int floorSeed;

    public FloorContentProfile contentProfile;

    public Vector2Int currentGridPosition;

    public Vector2Int spawnPosition;

    public EncounterData currentEncounter;

    public bool currentEncounterIsCorrupted;

    public int timeElapsed = 0;
    public int nextCorruptionTimeThreshold = 5;

    public int TurnsUntilCorruption => nextCorruptionTimeThreshold - timeElapsed;

    public List<Vector2Int> clearedCombatTiles = new();
    public List<Vector2Int> clearedCorruptionTiles = new();
    public List<Vector2Int> clearedEventTiles = new();

    public List<Vector2Int> discoveredEventTiles = new();

    public Dictionary<Vector2Int, List<int>> usedEventOptions = new();

    public List<ShopData> shops = new();

    public List<ChestData> chests = new();

    public int corruptionRadius = -1;

    public bool hasShownFloorIntro = false;
    public bool hasInitializedSpawn = false;
    public bool bossDefeated = false;

    public PendingRewardType pendingReward;
}