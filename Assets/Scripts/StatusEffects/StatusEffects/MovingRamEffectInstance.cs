using UnityEngine;

public class MovingRamEffectInstance : StatusEffectInstance
{
    private MovingRamEffectData ramData;

    public override void OnApply()
    {
        ramData = data as MovingRamEffectData;

        if (ramData == null)
        {
            Debug.LogError(
                "MovingRamEffectInstance: Invalid data type!"
            );

            return;
        }

        UpdateStatModifiers();
    }

    protected override void UpdateStatModifiers()
    {
        if (ramData == null)
            return;

        SetStatModifier(
            ShipStatType.CollisionDamage,
            ramData.collisionDamageBonus,
            StatModifierOperation.PercentAdd
        );
    }

    public override void OnTurnEnd()
    {
        SetDuration(0);
    }
}
