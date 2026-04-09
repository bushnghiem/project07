using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public EffectTrigger trigger;

    public abstract void Execute(EffectContext context);
}