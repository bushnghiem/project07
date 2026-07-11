using UnityEngine;

public class Reward
{
    public RewardDefinition Definition { get; }

    public Reward(RewardDefinition definition)
    {
        Definition = definition;
    }

    public string Title => Definition.rewardName;
    public string Description => Definition.description;
    public Sprite Icon => Definition.icon;

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

                PlayerSelectionUI.Instance.Open(
                    RewardManager.Instance.shipHolder.allPlayers,
                    player =>
                    {
                        RewardManager.Instance.AddItemToPlayer(
                            player,
                            Definition.item);

                        RewardMenuUI.Instance.FinishReward();
                    });

                return;

            case RewardType.Ship:

                RunManager.Instance.CurrentRun.team.Add(Definition.ship);

                break;
        }

        RewardMenuUI.Instance.FinishReward();
    }
}