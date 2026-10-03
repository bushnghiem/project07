using UnityEngine;
using System.Collections.Generic;

public static class ShipStatDefinitions
{
    private static readonly Dictionary<ShipStatType, StatDefinition> definitions = new()
    {
        {
            ShipStatType.MaxHealth,
            new StatDefinition(
                "Max Health",
                "The maximum amount of health this ship can have."
            )
        },

        {
            ShipStatType.StartingShield,
            new StatDefinition(
                "Starting Shield",
                "The amount of shield the ship starts with."
            )
        },

        {
            ShipStatType.MaxCharges,
            new StatDefinition(
                "Max Charges",
                "The maximum number of charges this ship can hold."
            )
        },

        {
            ShipStatType.ShotStrength,
            new StatDefinition(
                "Shot Strength",
                "Determines the maximum distance and force of the ship's shots."
            )
        },

        {
            ShipStatType.MoveStrength,
            new StatDefinition(
                "Move Strength",
                "Determines the maximum distance and force of ship's movements."
            )
        },

        {
            ShipStatType.Mass,
            new StatDefinition(
                "Mass",
                "Determines how the ship responds to knockback and movement."
            )
        },

        {
            ShipStatType.Initiative,
            new StatDefinition(
                "Initiative",
                "Determines the ship's position in the turn order."
            )
        },

        {
            ShipStatType.CollisionDamage,
            new StatDefinition(
                "Collision Damage",
                "The damage dealt when this ship collides with another object."
            )
        },

        {
            ShipStatType.CollisionKnockback,
            new StatDefinition(
                "Collision Knockback",
                "The knockback force applied by this ship during collisions."
            )
        },

        {
            ShipStatType.ProjectileDamage,
            new StatDefinition(
                "Projectile Damage",
                "Modifies the amount of damage to this ship's projectile."
            )
        },

        {
            ShipStatType.ProjectileKnockback,
            new StatDefinition(
                "Projectile Knockback",
                "Modifies the amount of knockback force to this ship's projectile."
            )
        },

        {
            ShipStatType.ProjectileMass,
            new StatDefinition(
                "Projectile Mass",
                "Modifies the amount of mass to this ship's projectile."
            )
        },

        {
            ShipStatType.ProjectileHealth,
            new StatDefinition(
                "Projectile Health",
                "Modifies the amount of health to this ship's projectile."
            )
        },

        {
            ShipStatType.ProjectileShield,
            new StatDefinition(
                "Projectile Shield",
                "Modifies the amount of starting shield to this ship's projectile."
            )
        },

        {
            ShipStatType.ActionPoints,
            new StatDefinition(
                "Action Points",
                "The number of action points available to the ship each turn."
            )
        },

        {
            ShipStatType.MoveAPCost,
            new StatDefinition(
                "Move AP Cost",
                "The number of action points required to move."
            )
        },

        {
            ShipStatType.ShootAPCostModifier,
            new StatDefinition(
                "Shoot AP Cost Modifier",
                "Modifies the action point cost of firing."
            )
        },

        {
            ShipStatType.CollisionResistance,
            new StatDefinition(
                "Collision Resistance",
                "Reduces damage taken from collisions."
            )
        },

        {
            ShipStatType.ExplosionResistance,
            new StatDefinition(
                "Explosion Resistance",
                "Reduces damage taken from explosions."
            )
        },

        {
            ShipStatType.DotResistance,
            new StatDefinition(
                "Damage Over Time Resistance",
                "Reduces damage taken from damage-over-time effects."
            )
        },

        {
            ShipStatType.FireResistance,
            new StatDefinition(
                "Fire Resistance",
                "Reduces damage taken from fire."
            )
        },

        {
            ShipStatType.ElectricResistance,
            new StatDefinition(
                "Electric Resistance",
                "Reduces damage taken from electric damage."
            )
        },

        {
            ShipStatType.IceResistance,
            new StatDefinition(
                "Ice Resistance",
                "Reduces damage taken from ice damage."
            )
        },

        {
            ShipStatType.PoisonResistance,
            new StatDefinition(
                "Poison Resistance",
                "Reduces damage taken from poison damage."
            )
        }
    };

    public static StatDefinition Get(ShipStatType statType)
    {
        if (definitions.TryGetValue(statType, out var definition))
            return definition;

        return new StatDefinition(
            statType.ToString(),
            "No description available."
        );
    }
}

public class StatDefinition
{
    public string DisplayName { get; }
    public string Description { get; }

    public StatDefinition(
        string displayName,
        string description)
    {
        DisplayName = displayName;
        Description = description;
    }
}

