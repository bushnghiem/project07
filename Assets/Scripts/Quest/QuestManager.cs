using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public GridManager CurrentGrid { get; private set; }

    public void RegisterGrid(GridManager grid)
    {
        CurrentGrid = grid;
    }

    public void UnregisterGrid(GridManager grid)
    {
        if (CurrentGrid == grid)
            CurrentGrid = null;
    }

    public void StartQuest(QuestData quest)
    {
        var run = RunManager.Instance.CurrentRun;

        QuestInstance instance = new QuestInstance();

        instance.quest = quest;
        instance.targetFloor =
            run.currentFloor + quest.floorsAhead;

        run.activeQuests.Add(instance);

        SaveManager.Instance.SaveRun();

        if (CurrentGrid != null)
        {
            ApplyQuestObjectives(CurrentGrid);
            CurrentGrid.GenerateVisuals();
        }

        Debug.Log("Started quest: " + quest.questName);
    }

    public void ApplyQuestObjectives(GridManager grid)
    {
        var run = RunManager.Instance.CurrentRun;

        foreach (var quest in run.activeQuests)
        {
            if (quest.completed)
                continue;

            if (quest.targetFloor != run.currentFloor)
                continue;

            if (quest.targetPosition != Vector2Int.zero)
            {
                grid.grid[quest.targetPosition.x,
                          quest.targetPosition.y]
                    .activeQuest = quest;

                continue;
            }

            Vector2Int pos = FindQuestTile(grid, quest);

            quest.targetPosition = pos;

            grid.grid[pos.x, pos.y].activeQuest = quest;
        }
    }

    Vector2Int FindQuestTile(GridManager grid, QuestInstance quest)
    {
        var run = RunManager.Instance.CurrentRun;

        int seed =
            run.runSeed ^
            (quest.targetFloor * 92837111) ^
            (quest.quest.questId * 689287499);

        System.Random rng = new System.Random(seed);

        List<Vector2Int> candidates = new();

        for (int x = 1; x < grid.width - 1; x++)
        {
            for (int y = 1; y < grid.height - 1; y++)
            {
                TileData tile = grid.grid[x, y];

                if (tile.tileType == TileType.Empty)
                {
                    candidates.Add(new Vector2Int(x, y));
                }
            }
        }

        // Fisher-Yates shuffle using the deterministic RNG
        for (int i = candidates.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
        }

        return candidates.Count > 0
            ? candidates[0]
            : Vector2Int.zero;
    }



    public void CompleteQuest(QuestInstance quest)
    {
        quest.completed = true;

        var run = RunManager.Instance.CurrentRun;

        RewardManager.Instance.AddRunCurrency(
            quest.quest.rewardCurrency);

        if (quest.quest.rewardItem != null)
        {
            // Give reward
        }

        SaveManager.Instance.SaveRun();

        Debug.Log("Quest Complete!");
    }

}

