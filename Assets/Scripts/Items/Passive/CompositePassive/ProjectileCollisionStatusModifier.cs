using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Projectile Collision Status")]
public class ProjectileCollisionStatusModifier
    : PassiveModifier
{
    public List<StatusEffectData> statusEffects = new();

    public override void Apply(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        foreach (var effect in statusEffects)
        {
            unit.AddProjectileCollisionStatus(
                effect);
        }
    }

    public override void Remove(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        foreach (var effect in statusEffects)
        {
            unit.RemoveProjectileCollisionStatus(
                effect);
        }
    }
}