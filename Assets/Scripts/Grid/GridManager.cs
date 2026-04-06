using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject combatPrefab;
    public GameObject eventPrefab;
    public GameObject portalPrefab;
    public GameObject shopPrefab;

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

    [Header("Safe Zone")]
    public int safeZoneRadius = 1;

    public bool IsGridReady { get; private set; }

    void Start()
    {
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
        ApplyRunModifications();

        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        if (!floor.hasInitializedSpawn)
        {
            Vector2Int spawnPos = GetSafeSpawnPosition();
            floor.currentGridPosition = spawnPos;
            floor.hasInitializedSpawn = true;
        }

        GenerateVisuals();
        IsGridReady = true;
    }

    public Vector2Int GetSafeSpawnPosition()
    {
        Vector2Int center = new Vector2Int(width / 2, height / 2);

        if (IsAreaSafe(center))
        {
            ClearArea(center);
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
                        ClearArea(pos);
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

    void ApplyRunModifications()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        foreach (var pos in floor.clearedCombatTiles)
        {
            if (IsInsideGrid(pos.x, pos.y))
                grid[pos.x, pos.y].tileType = TileType.Empty;
        }

        foreach (var pos in floor.clearedEventTiles)
        {
            if (IsInsideGrid(pos.x, pos.y))
            {
                grid[pos.x, pos.y].tileType = TileType.Empty;
                grid[pos.x, pos.y].assignedEvent = null;
            }
        }
    }

    public void clearEventTile(int x, int y)
    {
        if (IsInsideGrid(x, y))
        {
            grid[x, y].tileType = TileType.Empty;
            grid[x, y].assignedEvent = null;
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
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = GetWorldPosition(x, y);
                SpawnFeature(grid[x, y].tileType, worldPos);
            }
        }
    }

    private void SpawnFeature(TileType type, Vector3 position)
    {
        GameObject prefab = null;

        switch (type)
        {
            case TileType.Combat: prefab = combatPrefab; break;
            case TileType.Event: prefab = eventPrefab; break;
            case TileType.Portal: prefab = portalPrefab; break;
            case TileType.Shop: prefab = shopPrefab; break;
        }

        if (prefab != null)
        {
            Instantiate(prefab, position + Vector3.up * 0.5f, Quaternion.identity, transform);
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
}