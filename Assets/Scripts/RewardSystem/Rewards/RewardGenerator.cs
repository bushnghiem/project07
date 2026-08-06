using System;
using System.Collections.Generic;
using UnityEngine;

public static class RewardGenerator
{
    public static List<Reward> Generate(
        List<RewardDefinition> rewardPool,
        int amount)
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        return Generate(
            rewardPool,
            amount,
            RunManager.Instance.CurrentRun.runSeed + floor.floorIndex);
    }

    public static List<Reward> Generate(
        List<RewardDefinition> rewardPool,
        int amount,
        int seed)
    {
        List<RewardDefinition> pool = new(rewardPool);
        List<Reward> rewards = new();

        System.Random rng = new(seed);

        while (rewards.Count < amount && pool.Count > 0)
        {
            int index = rng.Next(pool.Count);

            rewards.Add(new Reward(pool[index]));

            pool.RemoveAt(index);
        }

        return rewards;
    }

    public static List<Reward> GenerateQuestRewards(
    QuestInstance quest)
    {
        return Generate(
            quest.quest.rewards,
            quest.quest.rewardsToChoose,
            QuestUtility.GetQuestSeed(quest));
    }
}