using UnityEngine;

public class ShockedEffectInstance : StatusEffectInstance
{
    private ShockedEffectData shockedData;

    public override void OnApply()
    {
        shockedData = data as ShockedEffectData;

        if (shockedData == null)
        {
            Debug.LogError(
                "ShockedEffectInstance: Invalid data type!"
            );

            return;
        }

        UpdateStatModifiers();
    }

    protected override void UpdateStatModifiers()
    {
        if (shockedData == null)
            return;

        float reduction =
            shockedData.actionPointsLostPerStack
            * Stacks;

        SetStatModifier(
            ShipStatType.ActionPoints,
            -reduction
        );
    }
}
