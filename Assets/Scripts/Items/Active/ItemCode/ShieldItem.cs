using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Shield")]
public class ShieldItem : ActiveItem
{
    public int shieldAmount = 1;

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
            unit.AddShield(shieldAmount);

            Debug.Log(
                $"{unit.gameObject.name} gained {shieldAmount} shield"
            );
        }
    }
}