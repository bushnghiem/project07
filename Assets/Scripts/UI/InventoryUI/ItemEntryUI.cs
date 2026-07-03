using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEntryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameText;
    public Image icon;

    private Item item;
    private FleetTooltipUI tooltip;

    public void Init(Item itemData, FleetTooltipUI tooltipUI)
    {
        item = itemData;
        tooltip = tooltipUI;

        nameText.text = item.itemName;

        if (icon != null)
            icon.sprite = item.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip == null || item == null)
            return;

        tooltip.Show(
            item.itemName,
            item.GetTooltipText()
        );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Hide();
    }
}