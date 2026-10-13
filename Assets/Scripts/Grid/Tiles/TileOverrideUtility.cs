using UnityEngine;

public static class TileOverrideUtility
{
    public static void ClearEncounter(Vector2Int pos)
    {
        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        TileOverride existing =
            floor.tileOverrides.Find(o => o.position == pos);

        if (existing == null)
        {
            existing = new TileOverride
            {
                position = pos,
                data = new TileOverrideData()
            };

            floor.tileOverrides.Add(existing);
        }

        // Preserve whether this was a corrupted encounter.
        bool wasCorrupted = existing.data.isCorrupted;

        // If necessary, also inspect the live tile before clearing it.
        GridManager grid = QuestManager.Instance != null
            ? QuestManager.Instance.CurrentGrid
            : null;

        if (grid != null && grid.IsInsideGrid(pos.x, pos.y))
        {
            wasCorrupted |= grid.grid[pos.x, pos.y].isCorrupted;
        }

        existing.data.hasTileTypeOverride = true;
        existing.data.tileType = TileType.Empty;

        existing.data.hasEncounterOverride = true;
        existing.data.encounter = null;

        existing.data.isElite = false;
        existing.data.isCorrupted = false;

        existing.data.corruptionCleared = wasCorrupted;

        SaveManager.Instance.SaveRun();
    }
}