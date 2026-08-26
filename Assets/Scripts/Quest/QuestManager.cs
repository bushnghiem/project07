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

        quest.objective.OnQuestStarted(
            instance,
            () =>
            {
                SaveManager.Instance.SaveRun();

                if (CurrentGrid != null)
                {
                    ApplyQuestObjectives(CurrentGrid);
                    CurrentGrid.GenerateVisuals();
                }

                Debug.Log(
                    "Started quest: " +
                    quest.questName);
            });
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
                grid.grid[
                    quest.targetPosition.x,
                    quest.targetPosition.y]
                    .activeQuest = quest;

                continue;
            }

            quest.quest.objective.PlaceObjective(grid, quest);
        }
    }


    public void CompleteQuest(QuestInstance quest)
    {
        if (quest.quest.rewards.Count > 0)
        {
            RewardMenuUI.Instance.Show(
                RewardGenerator.GenerateQuestRewards(quest),
                () => FinishQuest(quest));
        }
        else
        {
            FinishQuest(quest);
        }
    }

    private void FinishQuest(QuestInstance quest)
    {
        quest.completed = true;

        SaveManager.Instance.SaveRun();

        Debug.Log("Quest Complete!");
    }
}

