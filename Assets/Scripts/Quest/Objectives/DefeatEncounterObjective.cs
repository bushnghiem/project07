using UnityEngine;

[CreateAssetMenu]
public class DefeatEncounterObjective : QuestObjective
{
    public EncounterData encounter;

    public override void PlaceObjective(
        GridManager grid,
        QuestInstance quest)
    {
        Vector2Int pos =
            QuestUtility.FindEmptyTile(grid, quest);

        TileData tile = grid.grid[pos.x, pos.y];

        quest.targetPosition = pos;

        tile.tileType = TileType.Combat;
        tile.assignedEncounter = encounter;
        tile.activeQuest = quest;
    }

    public override bool TryComplete(
        QuestInstance quest,
        TileData tile)
    {
        return tile.assignedEncounter == null;
    }
}
