using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Shield")]
public class ShieldItem : ActiveItem
{
    public int shieldAmount = 1;

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

        if (data.targetUnit is UnitBase unitBase)
            unitBase.AddShield(shieldAmount);
    }
}
