
using UnityEngine;

public class DelayedEffectInstance : StatusEffectInstance
{
    private DelayedEffectData delayedData;
    private bool hasTriggered;

    public override void OnApply()
    {
        delayedData = data as DelayedEffectData;

        if (delayedData == null)
        {
            Debug.LogError(
                "DelayedEffectInstance: Invalid data type!"
            );
        }
    }

    public override void TickDuration()
    {
        if (hasTriggered)
            return;

        base.TickDuration();

        if (IsExpired)
        {
            TriggerDelayedEffects();
        }
    }

    private void TriggerDelayedEffects()
    {
        if (hasTriggered)
            return;

        hasTriggered = true;

        if (target == null || delayedData == null)
            return;

        Vector3 position = target.Position;

        UnitBase owningUnitBase = target as UnitBase;

        Entity sourceEntity = target;
        UnitBase owner = owningUnitBase;

        EffectContext context = new EffectContext(
            position,
            target.GameObject,
            sourceEntity,
            owner,
            sourceEntity != null
                ? sourceEntity.ActionContext
                : null
        );

        foreach (Effect effect in delayedData.effects)
        {
            if (effect == null)
                continue;

            effect.Execute(context);
        }
    }
}
