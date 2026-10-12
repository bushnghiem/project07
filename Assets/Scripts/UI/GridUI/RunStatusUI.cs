using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class RunStatusUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;
    public CanvasGroup canvasGroup;

    [Header("Fade")]
    public float fadeInSpeed = 20f;
    public float fadeOutSpeed = 8f;

    [Header("Texts")]
    public TMP_Text floorInfoText;
    public TMP_Text currencyText;
    public TMP_Text keyText;
    public TMP_Text timeText;
    public TMP_Text corruptionText;
    public TMP_Text bossText;
    public TMP_Text objectiveText;

    [Header("Active Quests")]
    public Transform questListContainer;
    public QuestEntryUI questEntryPrefab;
    public TMP_Text questHeaderText;

    private readonly List<QuestEntryUI> questEntries = new();

    void Start()
    {
        canvasGroup.alpha = 0f;
        panel.SetActive(true);
        Refresh();
    }

    void Update()
    {
        bool show = Input.GetKey(KeyCode.LeftShift);

        if (show)
            Refresh();

        float targetAlpha = show ? 1f : 0f;
        float speed = show ? fadeInSpeed : fadeOutSpeed;

        canvasGroup.alpha = Mathf.MoveTowards(
            canvasGroup.alpha,
            targetAlpha,
            speed * Time.deltaTime
        );
    }

    void Refresh()
    {
        var run = RunManager.Instance.CurrentRun;

        if (run == null)
            return;

        var floor = run.currentFloorData;

        if (floor == null || floor.contentProfile == null)
            return;

        floorInfoText.text =
            $"Sector {run.currentFloor + 1} // " +
            floor.contentProfile.floorName;

        currencyText.text = $"Units: {run.runCurrency}";
        keyText.text = $"Keys: {run.runKeys}";
        timeText.text = $"Time Elapsed: {floor.timeElapsed}";

        corruptionText.text =
            $"Corruption Grows in {floor.TurnsUntilCorruption} Moves";

        bool bossFloor = (run.currentFloor + 1) % 3 == 0;

        bossText.text = bossFloor
            ? "Boss Sector"
            : "Normal Sector";

        objectiveText.text = bossFloor
            ? "Defeat the Boss"
            : "Reach the Portal";

        RefreshQuests(run.activeQuests, run.currentFloor);
    }

    void RefreshQuests(
        List<QuestInstance> quests,
        int currentFloor)
    {
        foreach (var entry in questEntries)
        {
            if (entry != null)
                Destroy(entry.gameObject);
        }

        questEntries.Clear();

        int activeCount = 0;

        foreach (var quest in quests)
        {
            if (quest == null ||
                quest.quest == null)
            {
                continue;
            }

            activeCount++;

            QuestEntryUI entry = Instantiate(
                questEntryPrefab,
                questListContainer
            );

            entry.Setup(quest, currentFloor);
            questEntries.Add(entry);
        }

        if (questHeaderText != null)
        {
            questHeaderText.text =
                $"ACTIVE QUESTS ({activeCount})";
        }

        if (questListContainer != null)
        {
            questListContainer.gameObject.SetActive(
                activeCount > 0
            );
        }
    }
}