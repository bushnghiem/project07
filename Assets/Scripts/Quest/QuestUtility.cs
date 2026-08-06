using System;
using System.Collections.Generic;
using UnityEngine;

public static class QuestUtility
{
    static System.Random GetQuestRng(QuestInstance quest)
    {
        var run = RunManager.Instance.CurrentRun;

        int seed =
            run.runSeed ^
            (quest.targetFloor * 92837111) ^
            (quest.quest.questId * 689287499);

        return new System.Random(seed);
    }

    static Vector2Int PickRandom(List<Vector2Int> candidates, System.Random rng)
    {
        if (candidates.Count == 0)
            return Vector2Int.zero;

        return candidates[rng.Next(candidates.Count)];
    }

    public static Vector2Int FindEmptyTile(GridManager grid, QuestInstance quest)
    {
        var rng = GetQuestRng(quest);

        List<Vector2Int> candidates = new();

        for (int x = 1; x < grid.width - 1; x++)
        {
            for (int y = 1; y < grid.height - 1; y++)
            {
                if (grid.grid[x, y].tileType == TileType.Empty)
                    candidates.Add(new Vector2Int(x, y));
            }
        }

        return PickRandom(candidates, rng);
    }

    public static Vector2Int FindPortal(GridManager grid)
    {
        for (int x = 1; x < grid.width - 1; x++)
        {
            for (int y = 1; y < grid.height - 1; y++)
            {
                if (grid.grid[x, y].tileType == TileType.Portal)
                    return new Vector2Int(x, y);
            }
        }

        return Vector2Int.zero;
    }

    public static Vector2Int FindElite(GridManager grid, QuestInstance quest)
    {
        var rng = GetQuestRng(quest);

        List<Vector2Int> candidates = new();

        for (int x = 1; x < grid.width - 1; x++)
        {
            for (int y = 1; y < grid.height - 1; y++)
            {
                if (grid.grid[x, y].isElite)
                    candidates.Add(new Vector2Int(x, y));
            }
        }

        return PickRandom(candidates, rng);
    }

    public static Vector2Int FindCombat(GridManager grid, QuestInstance quest)
    {
        var rng = GetQuestRng(quest);

        List<Vector2Int> candidates = new();

        for (int x = 1; x < grid.width - 1; x++)
        {
            for (int y = 1; y < grid.height - 1; y++)
            {
                if (grid.grid[x, y].tileType == TileType.Combat)
                    candidates.Add(new Vector2Int(x, y));
            }
        }

        return PickRandom(candidates, rng);
    }

    public static int GetQuestSeed(QuestInstance quest)
    {
        var run = RunManager.Instance.CurrentRun;

        return
            run.runSeed ^
            (quest.targetFloor * 92837111) ^
            (quest.quest.questId * 689287499);
    }
}