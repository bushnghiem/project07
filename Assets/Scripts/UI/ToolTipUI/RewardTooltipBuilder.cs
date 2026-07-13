using UnityEngine;

public static class RewardTooltipBuilder
{
    public static TooltipData Build(Reward reward)
    {
        var def = reward.Definition;

        switch (def.rewardType)
        {
            case RewardType.Currency:
                return new TooltipData(
                    def.rewardName,
                    $"{def.description}\n\nGain {def.value} currency.");

            case RewardType.Keys:
                return new TooltipData(
                    def.rewardName,
                    $"{def.description}\n\nGain {def.value} keys.");

            case RewardType.HealAllPlayers:
                return new TooltipData(
                    def.rewardName,
                    $"{def.description}\n\nHeal all players for {def.value} HP.");

            case RewardType.Item:
                return new TooltipData(
                    def.item.itemName,
                    def.item.GetTooltipText());

            case RewardType.Ship:
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