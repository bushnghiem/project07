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

        var run = RunManager.Instance.CurrentRun;

        int maxRadius = Mathf.Max(grid.width, grid.height) / 2;

        if (run.corruptionRadius >= 0)
        {
            currentRadius = run.corruptionRadius;

            ApplyCorruption();
        }
        else
        {
            currentRadius = maxRadius;
            run.corruptionRadius = currentRadius;

            SaveManager.Instance.SaveRun();
        }
    }

    public void OnStepTaken()
    {
        var run = RunManager.Instance.CurrentRun;
        int steps = run.stepsTaken;

        if (steps % stepsPerShrink == 0)
        {
            currentRadius--;

            run.corruptionRadius = currentRadius;

            ApplyCorruption();

            SaveManager.Instance.SaveRun();
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

        grid.GenerateVisuals();
    }

    void CorruptTile(Vector2Int pos)
    {
        TileData tile = grid.grid[pos.x, pos.y];

        if (tile.tileType == TileType.Portal)
            return;

        if (tile.tileType == TileType.Wall)
            return;

        if (tile.tileType == TileType.Combat && tile.assignedEncounter != null)
            return;

        tile.tileType = TileType.Combat;
        tile.assignedEncounter = GetCorruptionEncounter(pos);
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