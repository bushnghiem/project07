using UnityEngine;

public static class RewardTooltipBuilder
{
    public static TooltipData Build(Reward reward)
    {
        if (reward == null ||
            reward.Definition == null)
        {
            return new TooltipData(
                "Unknown Reward",
                "Invalid reward.");
        }

        var def = reward.Definition;

        switch (def.rewardType)
        {
            case RewardType.Currency:

                return new TooltipData(
                    def.rewardName,
                    $"{def.description}\n\n" +
                    $"Gain {def.value} currency.");

            case RewardType.Keys:

                return new TooltipData(
                    def.rewardName,
                    $"{def.description}\n\n" +
                    $"Gain {def.value} keys.");

            case RewardType.HealAllPlayers:

                return new TooltipData(
                    def.rewardName,
                    $"{def.description}\n\n" +
                    $"Heal all players for {def.value} HP.");

            case RewardType.Item:
            case RewardType.RandomItem:

                if (reward.ResolvedItem == null)
                {
                    return new TooltipData(
                        def.rewardName,
                        def.description);
                }

                return new TooltipData(
                    reward.ResolvedItem.itemName,
                    reward.ResolvedItem.GetTooltipText());

            case RewardType.Ship:

                if (def.ship == null)
                {
                    return new TooltipData(
                        def.rewardName,
                        def.description);
                }

                return new TooltipData(
                    def.ship.uniqueID,
                    $"HP: {def.ship.currentHealth}" +
                    $"\nCharges: {def.ship.currentCharges}");

            default:

                return new TooltipData(
                    def.rewardName,
                    def.description);
        }
    }
}
