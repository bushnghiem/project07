using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatEntryUI
    : MonoBehaviour,
      IPointerEnterHandler,
      IPointerExitHandler
{
    public TMP_Text nameText;
    public TMP_Text valueText;

    private StatBreakdown breakdown;
    private FleetTooltipUI tooltip;

    public void Init(
        StatBreakdown breakdown,
        FleetTooltipUI tooltipUI)
    {
        this.breakdown = breakdown;
        tooltip = tooltipUI;

        StatDefinition definition =
            ShipStatDefinitions.Get(
                breakdown.statType
            );

        nameText.text =
            definition.DisplayName;

        valueText.text =
            breakdown.finalValue.ToString("0.##");
    }

    public void OnPointerEnter(
        PointerEventData eventData)
    {
        tooltip.ShowStat(breakdown);
    }

    public void OnPointerExit(
        PointerEventData eventData)
    {
        tooltip.Hide();
    }
}
