using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/SelfHeal")]
public class SelfHealItem : ActiveItem
{
    public int healAmount = 100;

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
            unit.Heal(healAmount);

            Debug.Log(
                $"{unit.gameObject.name} gained {healAmount} health"
            );
        }
    }
}