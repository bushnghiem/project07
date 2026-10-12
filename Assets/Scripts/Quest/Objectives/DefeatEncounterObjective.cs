using UnityEngine;

[CreateAssetMenu(menuName = "Quest Objectives/Defeat Encounter")]
public class DefeatEncounterObjective : QuestObjective
{
    public EncounterData encounter;

    public override void PlaceObjective(
        GridManager grid,
        QuestInstance quest)
    {
        Vector2Int pos =
            QuestUtility.FindEmptyTile(grid, quest);

        quest.targetPosition = pos;

        grid.ModifyTile(pos, tile =>
        {
            tile.tileType = TileType.Combat;
            tile.assignedEncounter = encounter;
            tile.activeQuest = quest;
        });
    }

    public override bool TryComplete(
        QuestInstance quest,
        TileData tile)
    {
        return tile.assignedEncounter == null;
    }

    public override string GetProgressText(
        QuestInstance quest,
        GridManager grid)
    {
        if (grid == null)
            return "Travel to the encounter location";

        Vector2Int pos = quest.targetPosition;
        TileData tile = grid.grid[pos.x, pos.y];

        if (tile.assignedEncounter == null)
            return "Encounter defeated";

        return "Defeat the hostile encounter";
    }
}
