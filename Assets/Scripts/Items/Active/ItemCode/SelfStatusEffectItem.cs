using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/SelfStatus")]
public class SelfStatusEffectItem : ActiveItem
{
    public StatusEffectData statusEffect;
    public int stacks = 1;

    public override ItemTargetType TargetType =>
        ItemTargetType.Self;

    public override void Execute(
        Unit user,
        ItemTargetData data,
        ActionContext context
    )
    {
        if (user is UnitBase unit)
        {
            StatusEffectController statusController =
            unit.GetComponent<StatusEffectController>();

            if (statusController == null)
                return;

            statusController.ApplyEffect(
                statusEffect,
                stacks
            );

            Debug.Log(
                $"{unit.gameObject.name} gained {statusEffect} effect"
            );
        }
    }
}
