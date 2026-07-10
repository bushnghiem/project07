using UnityEngine;

public static class PlayerTooltipBuilder
{
    public static TooltipData Build(Player player)
    {
        return new TooltipData(
            player.Name,
            $"HP: {player.CurrentHealth}/{player.MaxHealth}" +
            $"\nCharges: {player.CurrentCharges}/{player.MaxCharges}");
    }
}
