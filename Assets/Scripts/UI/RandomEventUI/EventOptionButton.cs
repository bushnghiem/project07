using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventOptionButton : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Button optionButton;

    private EventOption option;
    private EventUI eventUI;
    private int optionIndex;

    [SerializeField] private TooltipTrigger tooltipTrigger;

    public void Setup(EventOption option, int index, EventUI ui, bool isUsed)
    {
        this.option = option;
        this.optionIndex = index;
        this.eventUI = ui;

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

        tooltipTrigger.SetProvider(() => EventTooltipBuilder.Build(option));
    }

    void OnClick()
    {
        eventUI.SelectOption(option, optionIndex);
        GridUIManager.Instance.ClearState();
        FindFirstObjectByType<GridMovement>().inputLocked = false;
    }
}