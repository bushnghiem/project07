using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Apply Status Effect")]
public class ApplyStatusEffect : Effect
{
    public StatusEffectData statusEffect;
    public int stacks = 1;

    public override void Execute(EffectContext context)
    {
        if (context.owner == null)
            return;

        StatusEffectController statusController =
            context.owner.GetComponent<StatusEffectController>();

        if (statusController == null)
            return;

        statusController.ApplyEffect(
            statusEffect,
            stacks
        );
    }
}
