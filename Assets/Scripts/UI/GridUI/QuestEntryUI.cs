using TMPro;
using UnityEngine;

public class QuestEntryUI : MonoBehaviour
{
    [Header("Quest Text")]
    public TMP_Text questNameText;
    public TMP_Text questDescriptionText;
    public TMP_Text questTargetText;

    public void Setup(QuestInstance quest, int currentFloor)
    {
        if (quest == null || quest.quest == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        questNameText.text = quest.quest.questName;

        int targetSector = quest.targetFloor + 1;

        questTargetText.text = $"Target: Sector {targetSector}";

        switch (quest.state)
        {
            case QuestState.Active:
                SetupActiveQuest(quest, currentFloor, targetSector);
                break;

            case QuestState.Completed:
                questDescriptionText.text = "Objective completed.";
                break;

            case QuestState.Failed:
                questDescriptionText.text =
                    !string.IsNullOrWhiteSpace(quest.failureReason)
                        ? quest.failureReason
                        : "Quest failed.";
                break;
        }
    }

    private void SetupActiveQuest(
        QuestInstance quest,
        int currentFloor,
        int targetSector)
    {
        if (quest.targetFloor > currentFloor)
        {
            questDescriptionText.text =
                $"Travel to Sector {targetSector} to begin the objective.";
            return;
        }

        if (quest.targetFloor < currentFloor)
        {
            // The quest should normally already have been failed
            // by the floor-exit event. Don't invent a failure reason here.
            questDescriptionText.text =
                "The target sector has passed.";
            return;
        }

        GridManager grid =
            QuestManager.Instance != null
                ? QuestManager.Instance.CurrentGrid
                : null;

        questDescriptionText.text =
            quest.quest.objective.GetProgressText(quest, grid);
    }
}