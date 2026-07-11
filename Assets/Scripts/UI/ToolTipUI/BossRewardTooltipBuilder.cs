using UnityEngine;

public static class RewardTooltipBuilder
{
    public static TooltipData Build(Reward reward)
    {
        return new TooltipData(
            reward.Title,
            reward.Description);
    }
}