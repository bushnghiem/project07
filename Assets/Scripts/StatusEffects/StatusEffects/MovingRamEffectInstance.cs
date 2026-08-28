using UnityEngine;

public class MovingRamEffectInstance : StatusEffectInstance
{
    private MovingRamEffectData ramData;

    public override void OnApply()
    {
        ramData = data as MovingRamEffectData;
    }

    public override float ModifyStat(
        ShipStatType statType,
        float value)
    {
        if (statType != ShipStatType.CollisionDamage)
            return value;

        return value * (1f + ramData.collisionDamageBonus);
    }

    public override void OnTurnEnd()
    {
        SetDuration(0);
    }
}

