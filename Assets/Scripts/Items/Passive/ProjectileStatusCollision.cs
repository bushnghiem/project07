using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Items/Passive/Projectile Status")]
public class ProjectileStatusCollision : PassiveItem
{
    [System.Serializable]
    public class Entry
    {
        public StatusEffectData effect;
        public int stacks = 1;
    }

    public List<Entry> statusEffects = new();

    public override void ApplyEffect(Unit unit, PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var entry in statusEffects)
        {
            if (entry.effect == null)
                continue;

            unitBase.AddProjectileCollisionStatus(new AppliedStatusEffect
            {
                effect = entry.effect,
                stacks = entry.stacks
            });
        }
    }

    public override void RemoveEffect(Unit unit, PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var entry in statusEffects)
        {
            if (entry.effect == null)
                continue;

            unitBase.RemoveProjectileCollisionStatus(new AppliedStatusEffect
            {
                effect = entry.effect,
                stacks = entry.stacks
            });
        }
    }

    public override void ApplyEffect(Unit unit) { }
}