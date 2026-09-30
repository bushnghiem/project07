using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Tarred")]
public class TarredEffectData : StatusEffectData
{
    public int moveAPCostIncrease = 1;

    public override StatusEffectInstance CreateInstance(Unit target)
    {
        return new TarredEffectInstance();
    }
}
