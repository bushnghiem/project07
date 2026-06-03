using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Grant Projectile Effect")]
public class GrantProjectileEffect : PassiveItem
{
    public List<GrantedEffect> grantedEffects = new();

    public override void ApplyEffect(Unit unit)
    {
        // Not used
    }

    public override void ApplyEffect(
        Unit unit,
        PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var granted in grantedEffects)
        {
            if (granted.effect == null)
                continue;

            Effect runtimeEffect =
                Instantiate(granted.effect);

            runtimeEffect.trigger =
                granted.trigger;

            unitBase.AddProjectileRuntimeEffect(runtimeEffect);

            instance.injectedEffects.Add(runtimeEffect);
        }
    }

    public override void RemoveEffect(
        Unit unit,
        PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var effect in instance.injectedEffects)
        {
            unitBase.RemoveProjectileRuntimeEffect(effect);

            if (effect != null)
                Destroy(effect);
        }

        instance.injectedEffects.Clear();
    }
}