using UnityEngine;

public static class TileTooltipBuilder
{
    public static TooltipData Build(
        TileData tile,
        Vector2Int position,
        GridManager gridManager)
    {
        switch (tile.tileType)
        {
            case TileType.Combat:

                if (tile.isCorrupted)
                    return new TooltipData(
                        "Corrupted Encounter",
                        "Corrupted threat detected.");

                if (tile.isElite)
                    return new TooltipData(
                        "Elite Encounter",
                        "A powerful threat detected.");

                return new TooltipData(
                    "Combat",
                    "Threats detected.");

            case TileType.Event:

                if (gridManager.IsEventDiscovered(position))
                {
                    return new TooltipData(
                        "Known Event",
                        tile.assignedEvent.eventName);
                }

                return new TooltipData(
                    "Unknown Event",
                    "An unexplored Event.");

            case TileType.Shop:

                return new TooltipData(
                    "Shop",
                    "Purchase equipment.");

            case TileType.Portal:

                bool boss =
                    (RunManager.Instance.CurrentRun.currentFloor + 1) % 3 == 0;

                if (boss)
                {
                    return new TooltipData(
                        "Boss Gate",
                        "A massive threat awaits beyond this portal.");
                }

                return new TooltipData(
                    "Portal",
                    "Travel to the next sector.");
        }

        return new TooltipData("", "");
    }
}