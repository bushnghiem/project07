using UnityEngine;
using System.Collections.Generic;

public class EffectController : MonoBehaviour
{
    public List<Effect> effects;

    public void TriggerEffects(EffectTrigger trigger, Vector3 position, UnitBase owner)
    {
        Entity entity = GetComponent<Entity>();

        EffectContext context = new EffectContext(
            position,
            gameObject,
            entity,
            owner
        );

        TriggerEffects(trigger, context);
    }

    public void TriggerEffects(EffectTrigger trigger, EffectContext context)
    {
        foreach (var effect in effects)
        {
            if (effect.trigger != trigger)
                continue;

            effect.Execute(context);
        }
    }
}