using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Heal")]
public class HealItem : ActiveItem
{
    public int healAmount = 100;

    public override ItemTargetType TargetType =>
        ItemTargetType.Unit;

    public override void Execute(
        Unit user,
        ItemTargetData data
    )
    {
        if (data.targetUnit == null)
            return;

        data.targetUnit.Heal(healAmount);
    }
}
