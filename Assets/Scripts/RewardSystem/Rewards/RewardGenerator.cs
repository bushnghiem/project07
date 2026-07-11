using System;
using System.Collections.Generic;
using UnityEngine;

public static class RewardGenerator
{
    public static List<Reward> Generate(
        List<RewardDefinition> rewardPool,
        int amount)
    {
        List<RewardDefinition> pool = new(rewardPool);
        List<Reward> rewards = new();

        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        System.Random rng = new System.Random(
            RunManager.Instance.CurrentRun.runSeed +
            floor.floorIndex);

        while (rewards.Count < amount && pool.Count > 0)
        {
            int index = rng.Next(pool.Count);

            rewards.Add(new Reward(pool[index]));

            pool.RemoveAt(index);
        }

        return rewards;
    }
}