using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventOptionButton : MonoBehaviour
{
    public TextMeshProUGUI label;
    private EventOption option;
    private EventUI eventUI;
    public Button optionButton;

    public void Setup(EventOption option, EventUI ui)
    {
        this.option = option;
        this.eventUI = ui;

        if (label == null)
        {
            Debug.LogError("Label is NOT assigned on EventOptionButton!");
            return;
        }

        label.text = option.optionText;

        if (optionButton == null)
        {
            Debug.LogError("Button component missing!");
            return;
        }

        optionButton.onClick.RemoveAllListeners();
        optionButton.onClick.AddListener(OnClick);
        optionButton.onClick.AddListener(() => Debug.Log("Clicked!"));
    }

    void OnClick()
    {
        eventUI.SelectOption(option);
    }
}