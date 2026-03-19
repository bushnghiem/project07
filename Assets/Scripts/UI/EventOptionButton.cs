using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventOptionButton : MonoBehaviour
{
    public TextMeshProUGUI label;
    private EventOption option;
    private EventUI eventUI;
    public Button optionButton;

    private int optionIndex;

    public void Setup(EventOption option, int index, EventUI ui, bool isUsed)
    {
        this.option = option;
        this.optionIndex = index;
        this.eventUI = ui;

        if (label == null)
        {
            Debug.LogError("Label is NOT assigned on EventOptionButton!");
            return;
        }

        if (optionButton == null)
        {
            Debug.LogError("Button component missing!");
            return;
        }

        if (isUsed)
        {
            label.text = option.optionText + " (used)";
            optionButton.interactable = false;
        }
        else
        {
            label.text = option.optionText;
            optionButton.interactable = true;
        }

        optionButton.onClick.RemoveAllListeners();
        optionButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        eventUI.SelectOption(option, optionIndex);
    }
}