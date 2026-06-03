using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Projectile Collision Status")]
public class ProjectileCollisionStatusModifier : PassiveModifier
{
    [System.Serializable]
    public class Entry
    {
        public StatusEffectData effect;
        public int stacks = 1;
    }

    public List<Entry> statusEffects = new();

    public override void Apply(UnitBase unit, PassiveItemInstance instance)
    {
        foreach (var entry in statusEffects)
        {
            if (entry.effect == null)
                continue;

            unit.AddProjectileCollisionStatus(new AppliedStatusEffect
            {
                effect = entry.effect,
                stacks = entry.stacks
            });
        }
    }

    public override void Remove(UnitBase unit, PassiveItemInstance instance)
    {
        foreach (var entry in statusEffects)
        {
            if (entry.effect == null)
                continue;

            unit.RemoveProjectileCollisionStatus(new AppliedStatusEffect
            {
                effect = entry.effect,
                stacks = entry.stacks
            });
        }
    }
}