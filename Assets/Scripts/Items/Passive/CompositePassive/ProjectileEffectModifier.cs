using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Projectile Effects")]
public class ProjectileEffectModifier : PassiveModifier
{
    public List<GrantedEffect> grantedEffects = new();

    public override void Apply(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        foreach (var granted in grantedEffects)
        {
            if (granted.effect == null)
                continue;

            Effect runtimeEffect =
                Instantiate(granted.effect);

            runtimeEffect.trigger =
                granted.trigger;

            unit.AddProjectileRuntimeEffect(
                runtimeEffect);

            instance.injectedEffects.Add(
                runtimeEffect);
        }
    }

    public override void Remove(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        foreach (var effect in instance.injectedEffects)
        {
            unit.RemoveProjectileRuntimeEffect(
                effect);

            if (effect != null)
                Destroy(effect);
        }

        instance.injectedEffects.Clear();
    }
}