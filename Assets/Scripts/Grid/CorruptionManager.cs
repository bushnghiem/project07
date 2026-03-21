using UnityEngine;
using System.Collections.Generic;

public class CorruptionManager : MonoBehaviour
{
    public static CorruptionManager Instance;

    public int stepsPerShrink = 5;
    public int currentRadius;

    private GridManager grid;
    public EncounterPool corruptionEncounterPool;

    void Awake()
    {
        Instance = this;
    }

    public void Init(GridManager gridManager)
    {
        grid = gridManager;

        currentRadius = Mathf.Max(grid.width, grid.height) / 2;
    }

    public void OnStepTaken()
    {
        int steps = RunManager.Instance.CurrentRun.stepsTaken;

        if (steps % stepsPerShrink == 0)
        {
            currentRadius--;
            ApplyCorruption();
        }
    }

    void ApplyCorruption()
    {
        Vector2Int center = new Vector2Int(grid.width / 2, grid.height / 2);

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                float dist = Vector2Int.Distance(pos, center);

                if (dist > currentRadius)
                {
                    CorruptTile(pos);
                }
            }
        }
    }

    void CorruptTile(Vector2Int pos)
    {
        TileData tile = grid.grid[pos.x, pos.y];

        if (tile.tileType == TileType.Portal)
            return;

        if (tile.tileType == TileType.Wall)
            return;

        tile.tileType = TileType.Combat;

        tile.assignedEncounter = GetCorruptionEncounter(pos);

        grid.ClearEventVisualAt(pos.x, pos.y);
        grid.GenerateVisuals(); // maybe optimize later
    }

    EncounterData GetCorruptionEncounter(Vector2Int pos)
    {
        var run = RunManager.Instance.CurrentRun;

        int seed = run.runSeed
                   ^ (pos.x * 1234567)
                   ^ (pos.y * 7654321)
                   ^ 999999;

        System.Random rng = new System.Random(seed);

        var pool = corruptionEncounterPool.encounters;

        int index = rng.Next(pool.Count / 2, pool.Count);

        return pool[index];
    }
}