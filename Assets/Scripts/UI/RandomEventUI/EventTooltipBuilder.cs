using UnityEngine;
using System.Text;

public static class EventTooltipBuilder
{
    public static string Build(EventOption option)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var outcome in option.outcomes)
        {
            switch (outcome.type)
            {
                case OutcomeType.GainCurrency:
                    sb.AppendLine($"+{outcome.value} Scrap");
                    break;

                case OutcomeType.LoseCurrency:
                    sb.AppendLine($"-{outcome.value} Scrap");
                    break;

                case OutcomeType.HealPlayer:
                    sb.AppendLine($"Heal {outcome.value} HP");
                    break;

                case OutcomeType.DamagePlayer:
                    if (outcome.damage != null)
                    {
                        string damageText = $"Take {outcome.damage.amount:0}";

                        if (outcome.damage.element != DamageElement.None)
                        {
                            damageText += $" {outcome.damage.element}";
                        }

                        damageText += " Damage";

                        sb.AppendLine(damageText);
                    }
                    else
                    {
                        sb.AppendLine("Take Damage");
                    }
                    break;

                case OutcomeType.GiveItem:
                    if (outcome.item != null)
                        sb.AppendLine($"Gain {outcome.item.itemName}");
                    break;

                case OutcomeType.TakeTime:
                    sb.AppendLine($"Consumes {outcome.value} Moves");
                    break;

                case OutcomeType.StartCombat:
                    sb.AppendLine("Starts Combat");
                    break;

                case OutcomeType.Nothing:
                    sb.AppendLine("Nothing");
                    break;
            }
        }

        return sb.ToString();
    }
}