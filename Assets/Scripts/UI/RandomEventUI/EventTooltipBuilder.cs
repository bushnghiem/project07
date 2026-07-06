using System.Text;
using UnityEngine;

public static class EventTooltipBuilder
{
    public static string Build(EventOption option)
    {
        StringBuilder sb = new StringBuilder();

        if (option.outcomeGroups == null)
            return "";

        foreach (var group in option.outcomeGroups)
        {
            if (group == null)
                continue;

            // --- Group Header ---
            if (group.groupChance < 1f)
            {
                sb.AppendLine($"[{Mathf.RoundToInt(group.groupChance * 100)}% Risk]");
            }
            else if (!string.IsNullOrEmpty(group.groupName))
            {
                sb.AppendLine(group.groupName);
            }

            // Optional description (nice for clarity in roguelikes)
            if (!string.IsNullOrEmpty(group.description))
            {
                sb.AppendLine(group.description);
            }

            if (group.outcomes == null)
            {
                sb.AppendLine();
                continue;
            }

            // --- Outcomes ---
            foreach (var outcome in group.outcomes)
            {
                if (outcome == null)
                    continue;

                string chanceText =
                    outcome.chance < 1f
                    ? $"{Mathf.RoundToInt(outcome.chance * 100)}% "
                    : "";

                switch (outcome.type)
                {
                    case OutcomeType.GainCurrency:
                        sb.AppendLine($"{chanceText}+{outcome.value} Scrap");
                        break;

                    case OutcomeType.LoseCurrency:
                        sb.AppendLine($"{chanceText}-{outcome.value} Scrap");
                        break;

                    case OutcomeType.GainKeys:
                        sb.AppendLine($"{chanceText}+{outcome.value} Key{(outcome.value == 1 ? "" : "s")}");
                        break;

                    case OutcomeType.LoseKeys:
                        sb.AppendLine($"{chanceText}-{outcome.value} Key{(outcome.value == 1 ? "" : "s")}");
                        break;

                    case OutcomeType.HealPlayer:
                        sb.AppendLine($"{chanceText}Heal {outcome.value} HP");
                        break;

                    case OutcomeType.DamagePlayer:
                        if (outcome.damage != null)
                        {
                            string element =
                                outcome.damage.element == DamageElement.None
                                    ? ""
                                    : $"{outcome.damage.element} ";

                            sb.AppendLine(
                                $"{chanceText}Take {outcome.damage.amount:0} {element}Damage"
                            );
                        }
                        else
                        {
                            sb.AppendLine($"{chanceText}Take Damage");
                        }
                        break;

                    case OutcomeType.GiveItem:
                        if (outcome.item != null)
                            sb.AppendLine($"{chanceText}Gain {outcome.item.itemName}");
                        break;

                    case OutcomeType.GiveCharges:
                        sb.AppendLine($"{chanceText}Restore {outcome.value} Charge{(outcome.value == 1 ? "" : "s")}");
                        break;

                    case OutcomeType.StartCombat:
                        sb.AppendLine($"{chanceText}Start Combat");
                        break;

                    case OutcomeType.TakeTime:
                        sb.AppendLine($"{chanceText}Consumes {outcome.value} Time");
                        break;

                    case OutcomeType.Nothing:
                        sb.AppendLine($"{chanceText}Nothing");
                        break;
                }
            }

            sb.AppendLine(); // spacing between groups
        }

        return sb.ToString().TrimEnd();
    }
}