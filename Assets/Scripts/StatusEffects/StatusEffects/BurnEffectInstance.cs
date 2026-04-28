using UnityEngine;

public class BurnEffectInstance : StatusEffectInstance
{
    private BurnEffectData burnData;

    public override void OnApply()
    {
        burnData = data as BurnEffectData;

        if (burnData == null)
        {
            Debug.LogError("BurnEffectInstance: Invalid data type!");
        }
    }

    public override void OnTurnStart()
    {
        if (target == null)
        {
            Debug.Log("Burn: Target null");
            return;
        }

        float damage = burnData.damagePerStack * Stacks;

        Debug.Log($"Burn Damage: {damage}");

        target.Hurt(damage);
    }

    public override void OnTurnEnd()
    {
        SetStacks(Stacks - 1);

        if (Stacks <= 0)
        {
            SetDuration(0);
        }
    }
}