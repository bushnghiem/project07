using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Rusted")]
public class RustedEffectData : StatusEffectData
{
    [Tooltip("Extra incoming collision damage (percentage) per stack.")]
    public float damageTakenPerStack = 0.10f;

    public override StatusEffectInstance CreateInstance(Unit target)
    {
        return new RustedEffectInstance();
    }
}
