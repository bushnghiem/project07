using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Items/Passive/Status Collision")]
public class StatusCollision : PassiveItem
{
    [Header("Stat Settings")]
    public List<StatusEffectData> statusEffects = new();

    public override void ApplyEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            unitBase.SetCollisionStatusEffects(statusEffects);
        }
    }

    private static readonly List<StatusEffectData> EmptyEffects = new();

    public override void RemoveEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            unitBase.SetCollisionStatusEffects(EmptyEffects);
        }
    }
}
