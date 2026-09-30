using UnityEngine;

public class TarredEffectInstance : StatusEffectInstance
{
    private TarredEffectData tarredData;

    public override void OnApply()
    {
        tarredData = data as TarredEffectData;
    }

    public override float ModifyStat(
        ShipStatType statType,
        float value)
    {
        if (statType != ShipStatType.MoveAPCost)
            return value;

        return value + tarredData.moveAPCostIncrease;
    }
}
