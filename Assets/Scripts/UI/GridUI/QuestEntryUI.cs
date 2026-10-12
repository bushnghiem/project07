using TMPro;
using UnityEngine;

public class QuestEntryUI : MonoBehaviour
{
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

        questNameText.text = quest.quest.questName;

        int targetSector = quest.targetFloor + 1;

        questTargetText.text =
            $"Target: Sector {targetSector}";

        // Get objective-specific progress.
        if (quest.targetFloor > currentFloor)
        {
            questDescriptionText.text =
                $"Progress: Travel to Sector {targetSector}";
        }
        else if (quest.targetFloor < currentFloor)
        {
            questDescriptionText.text =
                "Failed: Target sector has passed";
        }
        else
        {
            GridManager grid =
                QuestManager.Instance != null
                ? QuestManager.Instance.CurrentGrid
                : null;

            questDescriptionText.text =
                "Progress: " +
                quest.quest.objective.GetProgressText(
                    quest, grid);
        }
    }
}