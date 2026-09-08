using System;
using System.Collections.Generic;

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
            RunManager.Instance.CurrentRun.runSeed
                + floor.floorIndex * 1000
                + 500);
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

            RewardDefinition definition = pool[index];

            rewards.Add(
                ResolveReward(definition, rng));

            pool.RemoveAt(index);
        }

        return rewards;
    }

    private static Reward ResolveReward(
        RewardDefinition definition,
        System.Random rng)
    {
        int value = ResolveValue(definition, rng);
        Item item = ResolveItem(definition, rng);

        return new Reward(
            definition,
            value,
            item);
    }

    private static int ResolveValue(
        RewardDefinition definition,
        System.Random rng)
    {
        if (definition.valueMode == RewardValueMode.Fixed)
            return definition.value;

        return rng.Next(
            definition.minValue,
            definition.maxValue + 1);
    }

    private static Item ResolveItem(
        RewardDefinition definition,
        System.Random rng)
    {
        if (definition.rewardType != RewardType.Item)
            return null;

        if (definition.itemMode == RewardItemMode.Fixed)
            return definition.item;

        var floor =
            RunManager.Instance.CurrentRun.currentFloorData;

        var pool = floor.contentProfile.floorItemPool;

        if (pool == null || pool.Count == 0)
            return null;

        return pool[rng.Next(pool.Count)];
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
