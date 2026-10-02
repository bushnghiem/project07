using UnityEngine;

public class IrradiatedEffectInstance
    : StatusEffectInstance
{
    private IrradiatedEffectData irradiatedData;

    private int radiationTurns = 0;

    public override void OnApply()
    {
        irradiatedData =
            data as IrradiatedEffectData;

        if (irradiatedData == null)
        {
            Debug.LogError(
                "IrradiatedEffectInstance: " +
                "Invalid data type!"
            );

            return;
        }

        UpdateStatModifiers();
    }

    protected override void UpdateStatModifiers()
    {
        if (irradiatedData == null)
            return;

        float maxHealthReduction =
            irradiatedData
                .maxHealthReductionPerStackPerTurn
            * Stacks
            * radiationTurns;

        SetStatModifier(
            ShipStatType.MaxHealth,
            -maxHealthReduction
        );
    }

    public override void OnTurnEnd()
    {
        if (target == null)
            return;

        radiationTurns++;

        UpdateStatModifiers();
        NotifyModifiersChanged();
    }

    public override void OnRemove()
    {
        base.OnRemove();

        radiationTurns = 0;
    }
}
