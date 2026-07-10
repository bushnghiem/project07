using UnityEngine;

public static class BossRewardTooltipBuilder
{
    public static TooltipData Build(BossReward reward)
    {
        return new TooltipData(
            reward.Title,
            reward.Description);
    }
}