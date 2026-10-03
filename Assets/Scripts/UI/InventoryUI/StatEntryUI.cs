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
    private StatBreakdown breakdown;

    public void Init(
        StatBreakdown breakdown,
        FleetTooltipUI tooltipUI)
    {
        this.breakdown = breakdown;
        tooltip = tooltipUI;

        StatDefinition definition =
            ShipStatDefinitions.Get(breakdown.StatType);

        nameText.text = definition.DisplayName;
        valueText.text =
            breakdown.FinalValue.ToString("0.##");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowStat(breakdown);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}