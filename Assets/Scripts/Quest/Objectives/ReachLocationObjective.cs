using UnityEngine;

[CreateAssetMenu]
public class ReachLocationObjective : QuestObjective
{
    public override void PlaceObjective(GridManager grid,
                                    QuestInstance quest)
    {
        Vector2Int pos = QuestUtility.FindEmptyTile(grid, quest);

        quest.targetPosition = pos;

        grid.grid[pos.x, pos.y].activeQuest = quest;
    }

    public override bool TryComplete(
        QuestInstance quest,
        TileData tile)
    {
        return tile.activeQuest == quest;
    }
}
