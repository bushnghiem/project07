using UnityEngine;

public class BossReward
{
    public BossRewardDefinition Definition { get; }

    public BossReward(BossRewardDefinition definition)
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
            case BossRewardType.Currency:
                RewardManager.Instance.AddRunCurrency(
                    Definition.value);
                break;

            case BossRewardType.Keys:
                RewardManager.Instance.AddRunKeys(
                    Definition.value);
                break;

            case BossRewardType.HealAllPlayers:
                RewardManager.Instance.HealAllPlayers(
                    Definition.value);
                break;

            case BossRewardType.Item:

                PlayerSelectionUI.Instance.Open(
                    RewardManager.Instance.shipHolder.allPlayers,
                    player =>
                    {
                        RewardManager.Instance.AddItemToPlayer(
                            player,
                            Definition.item);

                        BossRewardUI.Instance.FinishReward();
                    });

                return;

            case BossRewardType.Ship:

                RunManager.Instance.CurrentRun.team.Add(Definition.ship);

                break;
        }

        BossRewardUI.Instance.FinishReward();
    }
}