using UnityEngine;
using TMPro;

public class EventUI : MonoBehaviour
{
    public TextMeshProUGUI eventNameText;
    public TextMeshProUGUI descriptionText;

    public Transform optionsContainer;
    public GameObject optionButtonPrefab;

    private EventData currentEvent;

    private void Awake()
    {
        //gameObject.SetActive(false);
    }

    public void ShowEvent(EventData eventData)
    {
        if (eventData == null)
        {
            Debug.LogError("EventData is NULL");
            return;
        }

        if (eventNameText == null || descriptionText == null)
        {
            Debug.LogError("UI references not set!");
            return;
        }

        if (optionsContainer == null || optionButtonPrefab == null)
        {
            Debug.LogError("Options UI not set!");
            return;
        }

        currentEvent = eventData;

        eventNameText.text = eventData.eventName;
        descriptionText.text = eventData.description;

        ClearOptions();

        if (eventData.options == null || eventData.options.Count == 0)
        {
            Debug.LogWarning($"Event '{eventData.eventName}' has no options!");
            return;
        }

        foreach (var option in eventData.options)
        {
            if (option == null) continue;

            var buttonObj = Instantiate(optionButtonPrefab, optionsContainer);
            var button = buttonObj.GetComponent<EventOptionButton>();

            if (button == null)
            {
                Debug.LogError("OptionButton prefab missing script!");
                continue;
            }

            button.Setup(option, this);
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

    public void SelectOption(EventOption option)
    {
        EventManager.Instance.ExecuteOption(option);

        gameObject.SetActive(false);
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
                    // Example placeholder
                    break;
            }
        }

        return true;
    }
}