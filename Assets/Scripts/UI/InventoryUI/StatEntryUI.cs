using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatEntryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameText;
    public TMP_Text valueText;

    private string tooltipStatName;
    private string tooltipText;
    private FleetTooltipUI tooltip;

    public void Init(string statName, float value, FleetTooltipUI tooltipUI)
    {
        nameText.text = statName;
        valueText.text = value.ToString("0.##");

        tooltip = tooltipUI;
        tooltipStatName = $"{statName}";
        tooltipText = $"Value: {value}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(
            tooltipStatName,
            tooltipText
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}