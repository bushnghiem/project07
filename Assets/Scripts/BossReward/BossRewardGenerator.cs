using System.Collections.Generic;
using UnityEngine;

public static class BossRewardGenerator
{
    public static List<BossReward> Generate(int amount = 3)
    {
        var floor =
            RunManager.Instance.CurrentRun.currentFloorData;

        List<BossRewardDefinition> pool =
            new(floor.contentProfile.bossRewards);

        List<BossReward> rewards = new();

        System.Random rng =
            new System.Random(
                RunManager.Instance.CurrentRun.runSeed +
                floor.floorIndex);

        while (rewards.Count < amount && pool.Count > 0)
        {
            int index = rng.Next(pool.Count);

            rewards.Add(new BossReward(pool[index]));

            pool.RemoveAt(index);
        }

        return rewards;
    }
}