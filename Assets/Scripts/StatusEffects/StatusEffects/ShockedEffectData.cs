using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Shocked")]
public class ShockedEffectData : StatusEffectData
{
    public int actionPointsLostPerStack = 1;

    public override StatusEffectInstance CreateInstance(Unit target)
    {
        return new ShockedEffectInstance();
    }
}
