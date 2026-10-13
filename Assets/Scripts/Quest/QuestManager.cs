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
            if (!quest.IsActive)
                continue;

            if (quest.targetFloor != run.currentFloor)
                continue;

            if (quest.hasTargetPosition)
            {
                Vector2Int pos = quest.targetPosition;

                if (!grid.IsInsideGrid(pos.x, pos.y))
                {
                    Debug.LogWarning(
                        $"Invalid target position for quest: {quest.quest.questName}");

                    continue;
                }

                grid.grid[pos.x, pos.y].activeQuest = quest;
                continue;
            }

            quest.quest.objective.PlaceObjective(grid, quest);
        }
    }


    public bool CompleteQuest(QuestInstance quest)
    {
        if (quest == null || !quest.IsActive)
            return false;

        if (quest.quest.rewards != null &&
            quest.quest.rewards.Count > 0)
        {
            RewardMenuUI.Instance.Show(
                RewardGenerator.GenerateQuestRewards(quest),
                () => FinishQuest(quest));

            return true;
        }

        FinishQuest(quest);
        return true;
    }

    public bool FailQuest(
        QuestInstance quest,
        string reason = null)
    {
        if (quest == null || !quest.IsActive)
            return false;

        quest.state = QuestState.Failed;
        quest.failureReason = string.IsNullOrEmpty(reason)
            ? "The objective can no longer be completed."
            : reason;

        SaveManager.Instance.SaveRun();

        Debug.Log(
            $"Quest failed: {quest.quest.questName}. " +
            quest.failureReason);

        return true;
    }

    private void FinishQuest(QuestInstance quest)
    {
        if (quest == null || !quest.IsActive)
            return;

        quest.state = QuestState.Completed;

        SaveManager.Instance.SaveRun();

        Debug.Log(
            $"Quest completed: {quest.quest.questName}");
    }

    public void NotifyEvent(QuestEvent questEvent)
    {
        var run = RunManager.Instance.CurrentRun;

        foreach (var quest in run.activeQuests)
        {
            if (!quest.IsActive)
                continue;

            string failureReason =
                quest.quest.objective.HandleEvent(
                    quest,
                    questEvent);

            if (!string.IsNullOrEmpty(failureReason))
            {
                FailQuest(quest, failureReason);
            }
        }
    }
}

