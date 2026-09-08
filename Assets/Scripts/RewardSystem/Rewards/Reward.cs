using UnityEngine;

public class Reward
{
    public RewardDefinition Definition { get; }

    // The actual item this reward represents.
    // Used by both the UI and Claim().
    public Item ResolvedItem { get; private set; }

    public Reward(RewardDefinition definition)
    {
        Definition = definition;
    }

    public string Title
    {
        get
        {
            if ((Definition.rewardType == RewardType.Item ||
                 Definition.rewardType == RewardType.RandomItem) &&
                ResolvedItem != null)
            {
                return ResolvedItem.itemName;
            }

            return Definition.rewardName;
        }
    }

    public string Description
    {
        get
        {
            if ((Definition.rewardType == RewardType.Item ||
                 Definition.rewardType == RewardType.RandomItem) &&
                ResolvedItem != null)
            {
                return ResolvedItem.GetTooltipText();
            }

            return Definition.description;
        }
    }

    public Sprite Icon
    {
        get
        {
            if ((Definition.rewardType == RewardType.Item ||
                 Definition.rewardType == RewardType.RandomItem) &&
                ResolvedItem != null)
            {
                return ResolvedItem.icon;
            }

            return Definition.icon;
        }
    }

    public void SetResolvedItem(Item item)
    {
        ResolvedItem = item;
    }

    public void Claim()
    {
        switch (Definition.rewardType)
        {
            case RewardType.Currency:

                RewardManager.Instance.AddRunCurrency(
                    Definition.value);

                break;

            case RewardType.Keys:

                RewardManager.Instance.AddRunKeys(
                    Definition.value);

                break;

            case RewardType.HealAllPlayers:

                RewardManager.Instance.HealAllPlayers(
                    Definition.value);

                break;

            case RewardType.Item:
            case RewardType.RandomItem:

                if (ResolvedItem == null)
                {
                    Debug.LogError(
                        $"Reward '{Definition.rewardName}' " +
                        "has no resolved item.");

                    return;
                }

                PlayerSelectionUI.Instance.Open(
                    RewardManager.Instance.shipHolder.allPlayers,
                    player =>
                    {
                        RewardManager.Instance.AddItemToPlayer(
                            player,
                            ResolvedItem);

                        RewardMenuUI.Instance.FinishReward();
                    });

                return;

            case RewardType.Ship:

                if (Definition.ship == null)
                {
                    Debug.LogError(
                        $"Reward '{Definition.rewardName}' " +
                        "has no ship assigned.");

                    return;
                }

                RunManager.Instance.CurrentRun.team.Add(
                    Definition.ship);

                break;
        }

        RewardMenuUI.Instance.FinishReward();
    }
}
