using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Collision Status")]
public class UnitCollisionStatusModifier : PassiveModifier
{
    public List<StatusEffectData> statusEffects = new();

    private static readonly List<StatusEffectData>
        EmptyEffects = new();

    public override void Apply(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        unit.SetCollisionStatusEffects(
            statusEffects);
    }

    public override void Remove(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        unit.SetCollisionStatusEffects(
            EmptyEffects);
    }
}