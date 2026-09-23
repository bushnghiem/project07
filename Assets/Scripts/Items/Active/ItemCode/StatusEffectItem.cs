using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/StatusEffectApply")]
public class StatusEffectItem : ActiveItem
{
    public StatusEffectData statusEffect;
    public int stacks = 1;

    public override ItemTargetType TargetType =>
        ItemTargetType.Unit;

    public override void Execute(
        Unit user,
        ItemTargetData data,
        ActionContext context
    )
    {
        if (data.targetUnit == null)
            return;

        if (data.targetUnit is UnitBase unit)
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
