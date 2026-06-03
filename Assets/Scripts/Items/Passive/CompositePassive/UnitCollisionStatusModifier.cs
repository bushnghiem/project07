using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Collision Status")]
public class UnitCollisionStatusModifier : PassiveModifier
{
    [System.Serializable]
    public class Entry
    {
        public StatusEffectData effect;
        public int stacks = 1;
    }

    public List<Entry> statusEffects = new();

    private List<AppliedStatusEffect> runtimeCache = new();

    public override void Apply(UnitBase unit, PassiveItemInstance instance)
    {
        var runtimeCache = new List<AppliedStatusEffect>();

        foreach (var entry in statusEffects)
        {
            if (entry.effect == null)
                continue;

            runtimeCache.Add(new AppliedStatusEffect
            {
                effect = entry.effect,
                stacks = entry.stacks,
                sourceID = instance.itemData.itemID
            });
        }

        unit.AddCollisionStatusEffects(runtimeCache);
    }

    public override void Remove(UnitBase unit, PassiveItemInstance instance)
    {
        var runtimeCache = new List<AppliedStatusEffect>();

        foreach (var entry in statusEffects)
        {
            if (entry.effect == null)
                continue;

            runtimeCache.Add(new AppliedStatusEffect
            {
                effect = entry.effect,
                stacks = entry.stacks,
                sourceID = instance.itemData.itemID
            });
        }

        unit.RemoveCollisionStatusEffects(runtimeCache);
    }
}