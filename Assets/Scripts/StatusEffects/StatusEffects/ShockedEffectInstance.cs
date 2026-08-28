using UnityEngine;

public class ShockedEffectInstance : StatusEffectInstance
{
    private ShockedEffectData shockedData;

    public override void OnApply()
    {
        shockedData = data as ShockedEffectData;
    }

    public override float ModifyStat(
        ShipStatType statType,
        float value)
    {
        if (statType != ShipStatType.ActionPoints)
            return value;

        float reduction =
            shockedData.actionPointsLostPerStack * Stacks;

        return Mathf.Max(0f, value - reduction);
    }
}
