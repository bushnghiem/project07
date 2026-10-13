using UnityEngine;

[CreateAssetMenu(menuName = "Quest Objectives/Deliver Item")]
public class DeliverItemObjective : TileQuestObjective
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
        if (tile.activeQuest != quest)
            return false;

        Player deliveryPlayer = FindPlayerById(
            quest.deliveryPlayerId);

        if (deliveryPlayer == null)
            return false;

        if (!RewardManager.Instance.HasItem(
                deliveryPlayer,
                quest.quest.deliveryItem))
        {
            Debug.Log("Delivery item is missing.");
            return false;
        }

        bool removed =
            RewardManager.Instance.RemoveItemFromPlayer(
                deliveryPlayer,
                quest.quest.deliveryItem);

        if (!removed)
            return false;

        Debug.Log(
            $"Delivered {quest.quest.deliveryItem.name}.");

        return true;
    }

    private Player FindPlayerById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        foreach (Player player in
                 RewardManager.Instance.shipHolder.allPlayers)
        {
            if (player != null && player.DisplayName == id)
                return player;
        }

        return null;
    }

    public override void OnQuestStarted(
        QuestInstance quest,
        System.Action onComplete)
    {
        PlayerSelectionUI.Instance.Open(
            RewardManager.Instance.shipHolder.allPlayers,
            player =>
            {
                RewardManager.Instance.AddItemToPlayer(
                    player,
                    quest.quest.deliveryItem);

                quest.deliveryPlayerId = player.DisplayName;

                onComplete?.Invoke();
            });
    }

    public override string GetProgressText(
        QuestInstance quest,
        GridManager grid)
    {
        Player deliveryPlayer = FindPlayerById(
            quest.deliveryPlayerId);

        if (deliveryPlayer == null)
            return "The courier is gone";

        if (!RewardManager.Instance.HasItem(
                deliveryPlayer,
                quest.quest.deliveryItem))
        {
            return "The delivery item is missing";
        }

        return "Deliver the item at the marked location";
    }

    protected override string HandleSpecificEvent(
        QuestInstance quest,
        QuestEvent questEvent)
    {
        if (questEvent.type == QuestEventType.FloorExited &&
            questEvent.floor == quest.targetFloor)
        {
            return "The delivery location was abandoned.";
        }

        if (questEvent.type == QuestEventType.PlayerDied &&
            questEvent.playerId == quest.deliveryPlayerId)
        {
            Player player = FindPlayerById(
                quest.deliveryPlayerId);

            if (player == null ||
                !RewardManager.Instance.HasItem(
                    player,
                    quest.quest.deliveryItem))
            {
                return "The courier was lost with the delivery item.";
            }
        }

        if (questEvent.type == QuestEventType.ItemLost &&
            questEvent.playerId == quest.deliveryPlayerId &&
            questEvent.item == quest.quest.deliveryItem)
        {
            return "The required delivery item was lost.";
        }

        return null;
    }
}