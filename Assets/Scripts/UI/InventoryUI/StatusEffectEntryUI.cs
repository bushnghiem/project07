using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusEffectEntryUI :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text stacksText;

    private StatusEffectInstance effect;
    private FleetTooltipUI tooltip;

    public void Init(
        StatusEffectInstance effectInstance,
        FleetTooltipUI tooltipUI)
    {
        effect = effectInstance;
        tooltip = tooltipUI;

        Refresh();
    }

    public void Refresh()
    {
        if (effect == null || effect.data == null)
            return;

        // Icon
        if (icon != null)
        {
            icon.sprite = effect.data.statusIcon;
            icon.enabled = effect.data.statusIcon != null;
        }

        // Name
        if (nameText != null)
        {
            nameText.text = effect.data.displayName;
        }

        // Duration: 10T = 10 turns
        if (durationText != null)
        {
            durationText.text = effect.RemainingDuration < 0
                ? "∞"
                : $"{effect.RemainingDuration}T";
        }

        // Stacks: x4 = 4 stacks
        if (stacksText != null)
        {
            stacksText.text = effect.data.isStackable &&
                              effect.Stacks > 1
                ? $"x{effect.Stacks}"
                : "";
        }
    }

    public void OnPointerEnter(
        PointerEventData eventData)
    {
        if (effect == null || tooltip == null)
            return;

        tooltip.Show(
            effect.data.displayName,
            GetTooltipText()
        );
    }

    public void OnPointerExit(
        PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Hide();
    }

    private string GetTooltipText()
    {
        string text = effect.data.description;

        text += "\n\nDuration: ";

        if (effect.RemainingDuration < 0)
            text += "Permanent";
        else
            text += $"{effect.RemainingDuration} turns";

        if (effect.data.isStackable)
            text += $"\nStacks: {effect.Stacks}";

        return text;
    }
}