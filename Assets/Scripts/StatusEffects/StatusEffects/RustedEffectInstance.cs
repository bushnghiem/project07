using UnityEngine;

public class RustedEffectInstance : StatusEffectInstance
{
    private RustedEffectData rustedData;

    public override void OnApply()
    {
        rustedData = data as RustedEffectData;
    }

    public override float ModifyIncomingDamage(
        DamageInfo damageInfo,
        float damage)
    {
        if (damageInfo.Category != DamageCategory.Collision)
            return damage;

        float multiplier =
            1f + rustedData.damageTakenPerStack * Stacks;

        return damage * multiplier;
    }
}
