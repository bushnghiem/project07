using UnityEngine;

public class Reward
{
    public RewardDefinition Definition { get; }

    public int Value { get; }
    public Item Item { get; }

    public Reward(
        RewardDefinition definition,
        int value = 0,
        Item item = null)
    {
        Definition = definition;
        Value = value;
        Item = item;
    }

    public string Title => Definition.rewardName;
    public string Description => Definition.description;
    public Sprite Icon => Definition.icon;

    public void Claim()
    {
        switch (Definition.rewardType)
        {
            case RewardType.Currency:

                RewardManager.Instance.AddRunCurrency(Value);

                break;

            case RewardType.Keys:

                RewardManager.Instance.AddRunKeys(Value);

                break;

            case RewardType.HealAllPlayers:

                RewardManager.Instance.HealAllPlayers(Value);

                break;

            case RewardType.Item:

                PlayerSelectionUI.Instance.Open(
                    RewardManager.Instance.shipHolder.allPlayers,
                    player =>
                    {
                        RewardManager.Instance.AddItemToPlayer(
                            player,
                            Item);

                        RewardMenuUI.Instance.FinishReward();
                    });

                return;

            case RewardType.Ship:

                RunManager.Instance.CurrentRun.team.Add(
                    Definition.ship);

                break;
        }

        RewardMenuUI.Instance.FinishReward();
    }
}
