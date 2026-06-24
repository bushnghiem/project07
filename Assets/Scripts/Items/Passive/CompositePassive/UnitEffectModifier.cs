using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Unit Effects")]
public class UnitEffectModifier : PassiveModifier
{
    public List<GrantedEffect> grantedEffects = new();

    public override void Apply(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        var controller =
            unit.GetComponent<EffectController>();

        if (controller == null)
            return;

        foreach (var granted in grantedEffects)
        {
            if (granted.effect == null)
                continue;

            Effect runtimeEffect =
                Instantiate(granted.effect);

            runtimeEffect.trigger =
                granted.trigger;

            controller.effects.Add(runtimeEffect);

            instance.grantedObjects.Add(runtimeEffect);
        }
    }

    public override void Remove(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        var controller =
            unit.GetComponent<EffectController>();

        if (controller == null)
            return;

        foreach (var obj in instance.grantedObjects)
        {
            if (obj is Effect effect)
            {
                controller.effects.Remove(effect);

                Destroy(effect);
            }
        }
    }
}