using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventOptionButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public TextMeshProUGUI label;
    public Button optionButton;

    private EventOption option;
    private EventUI eventUI;
    private int optionIndex;

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
    }

    void OnClick()
    {
        eventUI.SelectOption(option, optionIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventTooltipUI.Instance.Show(
            EventTooltipBuilder.Build(option)
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventTooltipUI.Instance.Hide();
    }
}