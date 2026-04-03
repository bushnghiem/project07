using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EventUI : MonoBehaviour
{
    public TextMeshProUGUI eventNameText;
    public TextMeshProUGUI descriptionText;

    public Transform optionsContainer;
    public GameObject optionButtonPrefab;

    private EventData currentEvent;

    public void ShowEvent(EventData eventData)
    {
        if (eventData == null)
        {
            Debug.LogError("EventData is NULL");
            return;
        }

        currentEvent = eventData;

        eventNameText.text = eventData.eventName;
        descriptionText.text = eventData.description;

        ClearOptions();

        var run = RunManager.Instance.CurrentRun;
        var pos = run.currentFloorData.currentGridPosition;

        for (int i = 0; i < eventData.options.Count; i++)
        {
            var option = eventData.options[i];
            if (option == null) continue;

            if (!AreConditionsMet(option)) continue;

            bool isUsed = IsOptionUsed(i, option);

            var buttonObj = Instantiate(optionButtonPrefab, optionsContainer);
            var button = buttonObj.GetComponent<EventOptionButton>();

            if (button == null)
            {
                Debug.LogError("OptionButton prefab missing script!");
                continue;
            }

            button.Setup(option, i, this, isUsed);
        }

        gameObject.SetActive(true);
    }

    void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectOption(EventOption option, int index)
    {
        var run = RunManager.Instance.CurrentRun;
        var pos = run.currentFloorData.currentGridPosition;

        if (option.removeAfterUse)
        {
            if (!run.currentFloorData.usedEventOptions.ContainsKey(pos))
            {
                run.currentFloorData.usedEventOptions[pos] = new List<int>();
            }

            if (!run.currentFloorData.usedEventOptions[pos].Contains(index))
            {
                run.currentFloorData.usedEventOptions[pos].Add(index);
            }
        }

        EventManager.Instance.ExecuteOption(option);

        gameObject.SetActive(false);
    }

    bool IsOptionUsed(int index, EventOption option)
    {
        if (!option.removeAfterUse) return false;

        var run = RunManager.Instance.CurrentRun;
        var pos = run.currentFloorData.currentGridPosition;

        if (!run.currentFloorData.usedEventOptions.ContainsKey(pos))
            return false;

        return run.currentFloorData.usedEventOptions[pos].Contains(index);
    }

    bool AreConditionsMet(EventOption option)
    {
        if (option.conditions == null || option.conditions.Count == 0)
            return true;

        var run = RunManager.Instance.CurrentRun;

        foreach (var condition in option.conditions)
        {
            switch (condition.type)
            {
                case ConditionType.HasCurrency:
                    if (run.runCurrency < condition.value)
                        return false;
                    break;

                case ConditionType.LowHealth:
                    // Implement if needed
                    break;
            }
        }

        return true;
    }
}