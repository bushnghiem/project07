using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Items/Passive/Status Collision")]
public class StatusCollision : PassiveItem
{
    [System.Serializable]
    public class Entry
    {
        public StatusEffectData effect;
        public int stacks = 1;
    }

    public List<Entry> statusEffects = new();

    private List<AppliedStatusEffect> runtimeCache = new();

    public override void ApplyEffect(Unit unit)
    {
        if (unit is not UnitBase unitBase)
            return;

        runtimeCache.Clear();

        foreach (var entry in statusEffects)
        {
            if (entry.effect == null)
                continue;

            runtimeCache.Add(new AppliedStatusEffect
            {
                effect = entry.effect,
                stacks = entry.stacks,
                sourceID = itemID
            });
        }

        unitBase.AddCollisionStatusEffects(runtimeCache);
    }

    public override void RemoveEffect(Unit unit)
    {
        if (unit is not UnitBase unitBase)
            return;

        unitBase.RemoveCollisionStatusEffects(runtimeCache);
    }
}