using TMPro;
using UnityEngine;

public class FleetTooltipUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private TMPTerminalTypewriter typewriter;

    private void Start()
    {
        Hide();
    }

    public void Show(
        string title,
        string description)
    {
        panel.SetActive(true);

        titleText.text = title;

        typewriter.ShowText(description);
    }

    public void ShowStat(
        StatBreakdown breakdown)
    {
        if (breakdown == null)
            return;

        StatDefinition definition =
            ShipStatDefinitions.Get(
                breakdown.statType);

        string text =
            definition.Description +
            "\n\n";

        foreach (var entry in breakdown.entries)
        {
            switch (entry.operation)
            {
                case StatModifierOperation.Flat:

                    text +=
                        $"{entry.sourceName}: " +
                        $"{FormatSigned(entry.value)}\n";

                    break;

                case StatModifierOperation.PercentAdd:

                    text +=
                        $"{entry.sourceName}: " +
                        $"{FormatPercent(entry.value)}\n";

                    break;

                case StatModifierOperation.PercentMultiply:

                    text +=
                        $"{entry.sourceName}: " +
                        $"×{entry.value:0.##}\n";

                    break;

                case StatModifierOperation.Override:

                    text +=
                        $"{entry.sourceName}: " +
                        $"Set to {entry.value:0.##}\n";

                    break;
            }
        }

        text +=
            $"\nFinal: {breakdown.finalValue:0.##}";

        Show(
            definition.DisplayName,
            text
        );
    }

    private string FormatSigned(float value)
    {
        return value >= 0f
            ? $"+{value:0.##}"
            : $"{value:0.##}";
    }

    private string FormatPercent(float value)
    {
        float percentage = value * 100f;

        return percentage >= 0f
            ? $"+{percentage:0.##}%"
            : $"{percentage:0.##}%";
    }

    public void Hide()
    {
        typewriter.StopTyping();
        panel.SetActive(false);
    }
}
