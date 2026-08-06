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
}
