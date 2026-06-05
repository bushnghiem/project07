using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatEntryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameText;
    public TMP_Text valueText;

    private string tooltipText;
    private FleetTooltipUI tooltip;

    public void Init(string statName, float value, FleetTooltipUI tooltipUI)
    {
        nameText.text = statName;
        valueText.text = value.ToString("0.##");

        tooltip = tooltipUI;
        tooltipText = $"{statName}\nValue: {value}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}