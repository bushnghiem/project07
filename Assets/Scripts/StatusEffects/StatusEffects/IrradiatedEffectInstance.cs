using UnityEngine;

public class IrradiatedEffectInstance : StatusEffectInstance
{
    private IrradiatedEffectData irradiatedData;

    // How many turns of radiation have occurred.
    private int radiationTurns = 0;

    public override void OnApply()
    {
        irradiatedData = data as IrradiatedEffectData;

        if (irradiatedData == null)
        {
            Debug.LogError(
                "IrradiatedEffectInstance: Invalid data type!"
            );
        }
    }

    public override void OnTurnEnd()
    {
        if (target == null)
        {
            Debug.LogWarning("Irradiated: Target null");
            return;
        }

        // Increase radiation severity.
        radiationTurns++;

        // Damage increases every turn and with stacks.
        float damage =
            irradiatedData.damagePerStackPerTurn
            * Stacks
            * radiationTurns;

        Debug.Log(
            $"Irradiated Damage: {damage} | " +
            $"Stacks: {Stacks} | " +
            $"Radiation Turns: {radiationTurns}"
        );

        target.Hurt(
            DamagePresets.Radiation(damage)
        );
    }

    public override float ModifyStat(
        ShipStatType statType,
        float value)
    {
        if (statType != ShipStatType.MaxHealth)
            return value;

        float reduction =
            irradiatedData.maxHealthReductionPerStackPerTurn
            * Stacks
            * radiationTurns;

        return Mathf.Max(0f, value - reduction);
    }

    public override void OnRemove()
    {
        radiationTurns = 0;
    }
}
