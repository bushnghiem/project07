using UnityEngine;

[CreateAssetMenu(menuName = "Quest Objectives/Visit Location")]
public class ReachLocationObjective : TileQuestObjective
{
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
            tile.activeQuest = quest;
        });
    }

    public override bool TryComplete(
        QuestInstance quest,
        TileData tile)
    {
        return tile.activeQuest == quest;
    }

    public override string GetProgressText(
        QuestInstance quest,
        GridManager grid)
    {
        return "Reach the marked location";
    }

    protected override string HandleSpecificEvent(
        QuestInstance quest,
        QuestEvent questEvent)
    {
        if (questEvent.type == QuestEventType.FloorExited &&
            questEvent.floor == quest.targetFloor)
        {
            return "The sector was abandoned before the location was reached.";
        }

        return null;
    }
}