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
    public UnityEngine.UI.Button questHeaderButton;

    private bool showAllQuests = false;

    private readonly Dictionary<QuestInstance, QuestEntryUI>
        questEntries = new();

    void Start()
    {
        if (questHeaderButton != null)
        {
            questHeaderButton.onClick.AddListener(ToggleQuestView);
        }

        canvasGroup.alpha = 0f;
        panel.SetActive(true);
        Refresh();
    }

    void OnDestroy()
    {
        if (questHeaderButton != null)
        {
            questHeaderButton.onClick.RemoveListener(
                ToggleQuestView
            );
        }
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

        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
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

        currencyText.text = $"Credits: {run.runCurrency}";
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
        HashSet<QuestInstance> visibleQuests = new();

        int activeCount = 0;

        foreach (var quest in quests)
        {
            if (quest == null || quest.quest == null)
                continue;

            if (quest.IsActive)
                activeCount++;

            // In active-only mode, hide completed and failed quests.
            if (!showAllQuests && !quest.IsActive)
                continue;

            visibleQuests.Add(quest);

            if (!questEntries.TryGetValue(
                    quest, out QuestEntryUI entry)
                || entry == null)
            {
                entry = Instantiate(
                    questEntryPrefab,
                    questListContainer
                );

                questEntries[quest] = entry;
            }

            entry.gameObject.SetActive(true);
            entry.Setup(quest, currentFloor);
        }

        // Hide entries that are not part of the current view.
        foreach (var pair in questEntries)
        {
            if (pair.Value == null)
                continue;

            pair.Value.gameObject.SetActive(
                visibleQuests.Contains(pair.Key)
            );
        }

        if (questHeaderText != null)
        {
            questHeaderText.text = showAllQuests
                ? $"ALL QUESTS ({quests.FindAll(q => q != null && q.quest != null).Count})"
                : $"ACTIVE QUESTS ({activeCount})";
        }

        if (questListContainer != null)
        {
            questListContainer.gameObject.SetActive(
                visibleQuests.Count > 0
            );
        }
    }

    public void RefreshTimeValue()
    {
        var run = RunManager.Instance.CurrentRun;

        if (run == null)
            return;

        var floor = run.currentFloorData;

        if (floor == null || floor.contentProfile == null)
            return;

        timeText.text = $"Time Elapsed: {floor.timeElapsed}";
    }

    void ToggleQuestView()
    {
        Debug.Log("Toggle Quests");
        showAllQuests = !showAllQuests;

        Refresh();
    }
}