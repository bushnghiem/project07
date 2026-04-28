using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Burn")]
public class BurnEffectData : StatusEffectData
{
    public float damagePerStack = 2f;

    public override StatusEffectInstance CreateInstance(Unit target)
    {
        return new BurnEffectInstance();
    }
}
