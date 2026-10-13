using UnityEngine;

[CreateAssetMenu(menuName = "Quest Objectives/Defeat Encounter")]
public class DefeatEncounterObjective : TileQuestObjective
{
    public EncounterData encounter;

    public override void PlaceObjective(
        GridManager grid,
        QuestInstance quest)
    {
        Vector2Int? pos = QuestUtility.FindEmptyTile(grid, quest);

        if (!pos.HasValue)
        {
            Debug.LogWarning(
                $"No valid tile found for quest: {quest.quest.questName}");

            return;
        }

        quest.targetPosition = pos.Value;
        quest.hasTargetPosition = true;

        grid.ModifyTile(pos.Value, tile =>
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
        return tile.activeQuest == quest &&
               tile.assignedEncounter == null;
    }

    public override string GetProgressText(
        QuestInstance quest,
        GridManager grid)
    {
        if (grid == null)
            return "Travel to the encounter location";

        Vector2Int pos = quest.targetPosition;

        if (!grid.IsInsideGrid(pos.x, pos.y))
            return "Encounter location unavailable";

        TileData tile = grid.grid[pos.x, pos.y];

        if (tile.assignedEncounter == null)
            return "Encounter defeated";

        return "Defeat the hostile encounter";
    }

    protected override string HandleSpecificEvent(
        QuestInstance quest,
        QuestEvent questEvent)
    {
        if (questEvent.type == QuestEventType.FloorExited &&
            questEvent.floor == quest.targetFloor)
        {
            return "The sector was abandoned before the encounter was defeated.";
        }

        return null;
    }
}