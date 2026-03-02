using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject combatPrefab;
    public GameObject eventPrefab;
    public GameObject portalPrefab;

    public int width = 20;
    public int height = 20;
    public float tileSize = 1f;

    public TileData[,] grid;
    public EncounterPool combatEncounterPool;

    private System.Random rng;

    // Probabilities for random tile types
    [Range(0, 100)] public int emptyChance = 50;
    [Range(0, 100)] public int combatChance = 20;
    [Range(0, 100)] public int eventChance = 30;

    public bool IsGridReady { get; private set; }

    void Start()
    {
        GenerateGrid(RunManager.Instance.CurrentRun.runSeed);
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

                // Make borders walls
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    grid[x, y].tileType = TileType.Wall;
                    continue;
                }

                // Randomize tiles and add encounter to combat tiles
                grid[x, y].tileType = RandomTileType();

                if (grid[x, y].tileType == TileType.Combat)
                {
                    grid[x, y].assignedEncounter = GetDeterministicEncounter(x, y);
                }
            }
        }

        PlacePortal();
        ApplyRunModifications();
        GenerateVisuals();
        IsGridReady = true;
    }

    private void PlacePortal()
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

    void ApplyRunModifications()
    {
        var run = RunManager.Instance.CurrentRun;

        foreach (var pos in run.clearedCombatTiles)
        {
            if (IsInsideGrid(pos.x, pos.y))
            {
                grid[pos.x, pos.y].tileType = TileType.Empty;
            }
        }
    }

    private TileType RandomTileType()
    {
        int roll = rng.Next(0, 100);

        if (roll < emptyChance) return TileType.Empty;
        if (roll < emptyChance + combatChance) return TileType.Combat;
        return TileType.Event; // the rest is Event
    }

    private EncounterData GetDeterministicEncounter(int x, int y)
    {
        var run = RunManager.Instance.CurrentRun;

        // Make seed for each tile using spatial hashing to generate same encounter at same tile for same seed
        int tileSeed = run.runSeed
                       ^ (x * 73856093)
                       ^ (y * 19349663);

        System.Random tileRng = new System.Random(tileSeed);

        var pool = combatEncounterPool.encounters;

        if (pool == null || pool.Count == 0)
            return null;

        int index = tileRng.Next(0, pool.Count);
        return pool[index];
    }

    public void GenerateVisuals()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = GetWorldPosition(x, y);

                // Spawn base floor
                //Instantiate(floorPrefab, worldPos, Quaternion.identity, transform);

                // Spawn feature on top
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