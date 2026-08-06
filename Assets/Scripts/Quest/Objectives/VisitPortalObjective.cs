using UnityEngine;

[CreateAssetMenu]
public class VisitPortalObjective : QuestObjective
{
    public override void PlaceObjective(GridManager grid,
                                    QuestInstance quest)
    {
        Vector2Int portal = QuestUtility.FindPortal(grid);

        quest.targetPosition = portal;

        grid.grid[portal.x, portal.y].activeQuest = quest;
    }

    public override bool TryComplete(
        QuestInstance quest,
        TileData tile)
    {
        return tile.tileType == TileType.Portal;
    }
}
