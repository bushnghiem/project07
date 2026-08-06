using UnityEngine;

public static class TileOverrideUtility
{
    public static void ClearEncounter(Vector2Int pos)
    {
        var floor =
            RunManager.Instance.CurrentRun.currentFloorData;

        TileOverride existing =
            floor.tileOverrides.Find(o => o.position == pos);

        if (existing == null)
        {
            existing = new TileOverride();
            existing.position = pos;
            existing.data = new TileOverrideData();

            floor.tileOverrides.Add(existing);
        }

        existing.data.hasTileTypeOverride = true;
        existing.data.tileType = TileType.Empty;

        existing.data.hasEncounterOverride = true;
        existing.data.encounter = null;

        existing.data.isElite = false;
        existing.data.isCorrupted = false;
    }
}