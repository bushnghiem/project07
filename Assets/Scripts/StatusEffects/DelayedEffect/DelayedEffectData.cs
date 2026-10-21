using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Status Effect/Delayed Effect")]
public class DelayedEffectData : StatusEffectData
{
    [Header("Delayed Execution")]
    public List<Effect> effects = new();

    public override StatusEffectInstance CreateInstance(Unit target)
    {
        return new DelayedEffectInstance();
    }
}
