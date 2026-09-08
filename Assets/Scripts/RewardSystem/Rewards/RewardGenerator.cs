using System;
using System.Collections.Generic;
using UnityEngine;

public static class RewardGenerator
{
    public static List<Reward> Generate(
        List<RewardDefinition> rewardPool,
        int amount)
    {
        var floor =
            RunManager.Instance.CurrentRun.currentFloorData;

        return Generate(
            rewardPool,
            amount,
            RunManager.Instance.CurrentRun.runSeed
            + floor.floorIndex * 1000
            + 500);
    }

    public static List<Reward> Generate(
        List<RewardDefinition> rewardPool,
        int amount,
        int seed)
    {
        List<RewardDefinition> pool =
            new(rewardPool);

        List<Reward> rewards =
            new();

        System.Random rng =
            new(seed);

        while (rewards.Count < amount &&
               pool.Count > 0)
        {
            int index =
                rng.Next(pool.Count);

            RewardDefinition definition =
                pool[index];

            pool.RemoveAt(index);

            Reward reward =
                new Reward(definition);

            ResolveReward(
                reward,
                rng);

            rewards.Add(reward);
        }

        return rewards;
    }

    private static void ResolveReward(
        Reward reward,
        System.Random rng)
    {
        switch (reward.Definition.rewardType)
        {
            case RewardType.Item:

                reward.SetResolvedItem(
                    reward.Definition.item);

                break;

            case RewardType.RandomItem:

                ResolveRandomItem(
                    reward,
                    rng);

                break;
        }
    }

    private static void ResolveRandomItem(
        Reward reward,
        System.Random rng)
    {
        var floor =
            RunManager.Instance.CurrentRun.currentFloorData;

        if (floor == null ||
            floor.contentProfile == null)
        {
            Debug.LogError(
                "Cannot generate RandomItem reward: " +
                "current floor or content profile is missing.");

            return;
        }

        var itemPool =
            floor.contentProfile.floorItemPool;

        if (itemPool == null ||
            itemPool.Count == 0)
        {
            Debug.LogError(
                "Cannot generate RandomItem reward: " +
                "current floor has no items in its item pool.");

            return;
        }

        Item item =
            itemPool[rng.Next(itemPool.Count)];

        reward.SetResolvedItem(item);
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
