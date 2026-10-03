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

    public void Show(string title, string description)
    {
        panel.SetActive(true);

        titleText.text = title;

        typewriter.ShowText(description);
    }

    public void ShowStat(
    StatBreakdown breakdown)
    {
        StatDefinition definition =
            ShipStatDefinitions.Get(breakdown.StatType);

        string text =
            definition.Description +
            "\n\n";

        foreach (var entry in breakdown.Entries)
        {
            if (entry.IsBase)
            {
                text +=
                    $"{entry.SourceName}: " +
                    $"{entry.FlatBonus:0.##}\n";

                continue;
            }

            if (Mathf.Abs(entry.FlatBonus) > 0.001f)
            {
                text +=
                    $"{entry.SourceName}: " +
                    $"{FormatSigned(entry.FlatBonus)}\n";
            }

            if (Mathf.Abs(entry.PercentBonus) > 0.001f)
            {
                text +=
                    $"{entry.SourceName}: " +
                    $"{FormatPercent(entry.PercentBonus)}\n";
            }
        }

        text +=
            $"\nFinal: {breakdown.FinalValue:0.##}";

        Show(
            definition.DisplayName,
            text
        );
    }

    private string FormatSigned(float value)
    {
        return value >= 0
            ? $"+{value:0.##}"
            : $"{value:0.##}";
    }

    private string FormatPercent(float value)
    {
        float percentage = value * 100f;

        return percentage >= 0
            ? $"+{percentage:0.##}%"
            : $"{percentage:0.##}%";
    }


    public void Hide()
    {
        typewriter.StopTyping();
        panel.SetActive(false);
    }

}
