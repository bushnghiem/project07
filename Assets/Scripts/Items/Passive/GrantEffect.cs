using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Grant Effect")]
public class GrantEffectPassive : PassiveItem
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

        var controller =
            unitBase.GetComponent<EffectController>();

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

    public override void RemoveEffect(
        Unit unit,
        PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        var controller =
            unitBase.GetComponent<EffectController>();

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