using UnityEngine;

[CreateAssetMenu(menuName = "Quest Objectives/Deliver Item")]
public class DeliverItemObjective : QuestObjective
{
    public override void PlaceObjective(
        GridManager grid,
        QuestInstance quest)
    {
        Vector2Int pos =
            QuestUtility.FindEmptyTile(grid, quest);

        quest.targetPosition = pos;

        grid.grid[pos.x, pos.y].activeQuest = quest;
    }

    public override bool TryComplete(
        QuestInstance quest,
        TileData tile)
    {
        if (tile.activeQuest != quest)
            return false;

        Player deliveryPlayer =
            FindPlayerById(quest.deliveryPlayerId);

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

    Player FindPlayerById(string id)
    {
        foreach (Player player in
                 RewardManager.Instance.shipHolder.allPlayers)
        {
            if (player.DisplayName == id)
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

                quest.deliveryPlayerId =
                    player.DisplayName;

                onComplete?.Invoke();
            }
        );
    }


}
