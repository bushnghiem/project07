using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject combatPrefab;
    public GameObject eliteCombatPrefab;
    public GameObject corruptionCombatPrefab;
    public GameObject eventPrefab;
    public GameObject portalPrefab;
    public GameObject shopPrefab;
    public GameObject chestPrefab;
    public GameObject questMarkerPrefab;

    public int width = 20;
    public int height = 20;
    public float tileSize = 1f;

    public TileData[,] grid;
    public EncounterPool combatEncounterPool;
    public EventPool eventPool;

    private System.Random rng;

    [Header("Tile Chances")]
    [Range(0, 100)] public int emptyChance = 50;
    [Range(0, 100)] public int combatChance = 20;
    [Range(0, 100)] public int eventChance = 30;

    [Range(0, 10)]
    public int maxShops = 3;

    [Range(0, 10)]
    public int maxChests = 2;

    [Header("Safe Zone")]
    public int safeZoneRadius = 1;

    public bool IsGridReady { get; private set; }

    void Start()
    {
        QuestManager.Instance.RegisterGrid(this);
        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        var profile = RunManager.Instance.GetProfileForFloor(run.currentFloor, run.runSeed);

        if (floor.contentProfile == null)
        {
            floor.contentProfile = profile;
        }

        GenerateGrid(floor.floorSeed);
    }

    public void GenerateGrid(int seed)
    {
        rng = new System.Random(seed);
        grid = new TileData[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new TileData();

                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    grid[x, y].tileType = TileType.Wall;
                    continue;
                }

                grid[x, y].tileType = RandomTileType();

                if (grid[x, y].tileType == TileType.Combat)
                    grid[x, y].assignedEncounter = GetDeterministicEncounter(x, y);

                if (grid[x, y].tileType == TileType.Event)
                    grid[x, y].assignedEvent = GetDeterministicEvent(x, y);
            }
        }

        PlacePortal();
        PlaceShops();
        PlaceChests();
        PlaceEliteEncounters();
        
        ApplyTileOverrides();

        QuestManager.Instance.ApplyQuestObjectives(this);

        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        if (!floor.hasInitializedSpawn)
        {
            Vector2Int spawnPos = GetSafeSpawnPosition();

            floor.spawnPosition = spawnPos;
            floor.currentGridPosition = spawnPos;

            ClearArea(spawnPos);

            floor.hasInitializedSpawn = true;
        }
        else
        {
            ClearArea(floor.spawnPosition);
        }

        GenerateVisuals();
        IsGridReady = true;
    }

    public Vector2Int GetSafeSpawnPosition()
    {
        Vector2Int center = new Vector2Int(width / 2, height / 2);

        if (IsAreaSafe(center))
        {
            return center;
        }

        for (int radius = 1; radius < Mathf.Max(width, height); radius++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector2Int pos = center + new Vector2Int(x, y);

                    if (!IsInsideGrid(pos.x, pos.y))
                        continue;

                    if (IsAreaSafe(pos))
                    {
                        return pos;
                    }
                }
            }
        }

        Debug.Log("No safe spawn found, defaulting to (1,1)");
        return new Vector2Int(1, 1);
    }

    bool IsAreaSafe(Vector2Int center)
    {
        for (int x = -safeZoneRadius; x <= safeZoneRadius; x++)
        {
            for (int y = -safeZoneRadius; y <= safeZoneRadius; y++)
            {
                int nx = center.x + x;
                int ny = center.y + y;

                if (!IsInsideGrid(nx, ny))
                    return false;

                var tile = grid[nx, ny];

                if (tile.tileType == TileType.Wall || tile.tileType == TileType.Portal)
                    return false;
            }
        }

        return true;
    }

    void ClearArea(Vector2Int center)
    {
        for (int x = -safeZoneRadius; x <= safeZoneRadius; x++)
        {
            for (int y = -safeZoneRadius; y <= safeZoneRadius; y++)
            {
                int nx = center.x + x;
                int ny = center.y + y;

                if (!IsInsideGrid(nx, ny))
                    continue;

                grid[nx, ny].tileType = TileType.Empty;
                grid[nx, ny].assignedEncounter = null;
                grid[nx, ny].assignedEvent = null;
            }
        }
    }

    void PlacePortal()
    {
        int x, y;

        do
        {
            x = rng.Next(1, width - 1);
            y = rng.Next(1, height - 1);
        }
        while (grid[x, y].tileType == TileType.Wall);

        grid[x, y].tileType = TileType.Portal;
    }

    void PlaceShops()
    {
        int placed = 0;
        int safety = 0;

        while (placed < maxShops && safety < 1000)
        {
            int x = rng.Next(1, width - 1);
            int y = rng.Next(1, height - 1);

            if (grid[x, y].tileType == TileType.Empty
                && !IsAdjacentToTileType(x, y, TileType.Shop)
                && !IsAdjacentToTileType(x, y, TileType.Portal))
            {
                grid[x, y].tileType = TileType.Shop;
                placed++;
            }

            safety++;
        }
    }

    void PlaceChests()
    {
        int placed = 0;
        int safety = 0;

        while (placed < maxChests && safety < 1000)
        {
            int x = rng.Next(1, width - 1);
            int y = rng.Next(1, height - 1);

            if (grid[x, y].tileType == TileType.Empty
                && !IsAdjacentToTileType(x, y, TileType.Shop)
                && !IsAdjacentToTileType(x, y, TileType.Portal)
                && !IsAdjacentToTileType(x, y, TileType.Chest))
            {
                grid[x, y].tileType = TileType.Chest;
                placed++;
            }

            safety++;
        }
    }

    bool IsAdjacentToTileType(int x, int y, TileType type)
    {
        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (IsInsideGrid(nx, ny))
            {
                if (grid[nx, ny].tileType == type)
                    return true;
            }
        }

        return false;
    }

    void PlaceEliteEncounters()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;
        var pool = floor.contentProfile.eliteEncounters;

        if (pool == null || pool.Count == 0)
            return;

        List<Vector2Int> combatTiles = new();

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (grid[x, y].tileType == TileType.Combat)
                    combatTiles.Add(new Vector2Int(x, y));
            }
        }

        int elitesToPlace = Mathf.Min(
            floor.contentProfile.eliteCount,
            combatTiles.Count);

        for (int i = 0; i < elitesToPlace; i++)
        {
            int tileIndex = rng.Next(combatTiles.Count);

            Vector2Int pos = combatTiles[tileIndex];
            combatTiles.RemoveAt(tileIndex);

            TileData tile = grid[pos.x, pos.y];

            tile.isElite = true;
            tile.assignedEncounter = pool[rng.Next(pool.Count)];
        }
    }

    public void clearEventTile(int x, int y)
    {
        if (IsInsideGrid(x, y))
        {
            var floor = RunManager.Instance.CurrentRun.currentFloorData;

            Vector2Int pos = floor.currentGridPosition;

            ModifyTile(pos, tile =>
            {
                tile.tileType = TileType.Empty;
                tile.assignedEvent = null;
            });
        }
    }

    public void ClearEventVisualAt(int x, int y)
    {
        Vector3 worldPos = GetWorldPosition(x, y);

        foreach (Transform child in transform)
        {
            if (Vector3.Distance(child.position, worldPos + Vector3.up * 0.5f) < 0.1f)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    private TileType RandomTileType()
    {
        int roll = rng.Next(0, 100);

        if (roll < emptyChance) return TileType.Empty;
        if (roll < emptyChance + combatChance) return TileType.Combat;
        return TileType.Event;
    }

    private EncounterData GetDeterministicEncounter(int x, int y)
    {
        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        int tileSeed = run.runSeed
                       ^ (x * 73856093)
                       ^ (y * 19349663);

        System.Random tileRng = new System.Random(tileSeed);

        var pool = floor.contentProfile.combatEncounters;

        if (pool == null || pool.Count == 0)
            return null;

        return pool[tileRng.Next(0, pool.Count)];
    }

    private EventData GetDeterministicEvent(int x, int y)
    {
        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        if (floor.contentProfile == null)
        {
            Debug.LogError("ContentProfile is NULL");
            return null;
        }

        var pool = floor.contentProfile.events;

        if (pool == null || pool.Count == 0)
        {
            Debug.LogError("Event pool is empty!");
            return null;
        }

        int seed = run.runSeed
                   ^ (x * 92837111)
                   ^ (y * 689287499);

        System.Random tileRng = new System.Random(seed);

        return pool[tileRng.Next(0, pool.Count)];
    }

    public void GenerateVisuals()
    {
        Debug.Log("Generated Visuals");
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = GetWorldPosition(x, y);
                SpawnFeature(grid[x, y], worldPos, x, y);
            }
        }
    }

    private void SpawnFeature(TileData tile, Vector3 position, int x, int y)
    {
        GameObject prefab = null;
        Quaternion rotation = Quaternion.identity;

        switch (tile.tileType)
        {
            case TileType.Combat:

                if (tile.isCorrupted)
                {
                    prefab = corruptionCombatPrefab;
                }
                else if (tile.isElite)
                {
                    prefab = eliteCombatPrefab;
                }
                else
                {
                    prefab = combatPrefab;
                }
                rotation = Quaternion.Euler(270f, 90f, 0f);
                break;

            case TileType.Event:
                prefab = eventPrefab;
                rotation = Quaternion.Euler(0f, 270f, 0f);
                break;

            case TileType.Portal:
                prefab = portalPrefab;
                rotation = Quaternion.Euler(0f, 0f, 0f);
                break;

            case TileType.Shop:
                prefab = shopPrefab;
                rotation = Quaternion.Euler(270f, 90f, 0f);
                break;

            case TileType.Chest:
                prefab = chestPrefab;
                rotation = Quaternion.Euler(270f, 90f, 0f);
                break;
        }

        if (prefab != null)
        {
            GameObject obj = Instantiate(
                prefab,
                position + Vector3.up * 0.5f,
                rotation,
                transform);

            TileHover hover = obj.AddComponent<TileHover>();
            hover.tile = tile;
            hover.gridPosition = new Vector2Int(x, y);
        }

        if (tile.activeQuest != null)
        {
            Instantiate(
                questMarkerPrefab,
                position + Vector3.up * 0.75f,
                Quaternion.identity,
                transform);
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0);
    }

    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public Vector3 GetGridCenterWorldPosition()
    {
        float centerX = (width - 1) * tileSize * 0.5f;
        float centerY = (height - 1) * tileSize * 0.5f;

        return new Vector3(centerX, centerY, 0f);
    }

    public bool IsEventDiscovered(Vector2Int position)
    {
        return RunManager.Instance.CurrentRun.currentFloorData
            .discoveredEventTiles
            .Contains(position);
    }

    public void ClearTileVisualAt(int x, int y)
    {
        Vector3 worldPos = GetWorldPosition(x, y);

        foreach (Transform child in transform)
        {
            if (Vector3.Distance(child.position,
                worldPos + Vector3.up * 0.5f) < 0.1f)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public void SpawnQuestMarker(Vector2Int pos)
    {
        Instantiate(
            questMarkerPrefab,
            GetWorldPosition(pos.x, pos.y) + Vector3.up * 0.75f,
            Quaternion.identity,
            transform);
    }

    TileOverride GetOverride(Vector2Int pos)
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        return floor.tileOverrides.Find(o => o.position == pos);
    }

    public void SaveTileOverride(Vector2Int pos)
    {
        var tile = grid[pos.x, pos.y];

        TileOverride existing = GetOverride(pos);

        if (existing == null)
        {
            existing = new TileOverride();
            existing.position = pos;

            RunManager.Instance.CurrentRun.currentFloorData
                .tileOverrides
                .Add(existing);
        }

        existing.data = new TileOverrideData
        {
            hasTileTypeOverride = true,
            tileType = tile.tileType,

            hasEncounterOverride = true,
            encounter = tile.assignedEncounter,

            hasEventOverride = true,
            assignedEvent = tile.assignedEvent,

            isElite = tile.isElite,
            isCorrupted = tile.isCorrupted
        };
    }

    void ApplyTileOverrides()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        foreach (var overrideTile in floor.tileOverrides)
        {
            TileData tile =
                grid[
                    overrideTile.position.x,
                    overrideTile.position.y];

            TileOverrideData data = overrideTile.data;

            if (data.hasTileTypeOverride)
                tile.tileType = data.tileType;

            if (data.hasEncounterOverride)
                tile.assignedEncounter = data.encounter;

            if (data.hasEventOverride)
                tile.assignedEvent = data.assignedEvent;

            tile.isElite = data.isElite;
            tile.isCorrupted = data.isCorrupted;
        }
    }

    public void ModifyTile(Vector2Int pos, System.Action<TileData> modify)
    {
        TileData tile = grid[pos.x, pos.y];

        modify(tile);

        SaveTileOverride(pos);

        GenerateVisuals();
    }
}