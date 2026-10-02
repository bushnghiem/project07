using UnityEngine;

public class TarredEffectInstance
    : StatusEffectInstance
{
    private TarredEffectData tarredData;

    public override void OnApply()
    {
        tarredData =
            data as TarredEffectData;

        if (tarredData == null)
        {
            Debug.LogError(
                "TarredEffectInstance: " +
                "Invalid data type!"
            );

            return;
        }

        UpdateStatModifiers();
    }

    protected override void UpdateStatModifiers()
    {
        if (tarredData == null)
            return;

        SetStatModifier(
            ShipStatType.MoveAPCost,
            tarredData.moveAPCostIncrease
        );
    }
}
