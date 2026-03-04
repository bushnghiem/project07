using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Shield")]
public class ShieldItem : ActiveItem
{
    public int shieldAmount = 1;

    public override void Activate(Unit user, Unit target)
    {
        if (target == null) return;

        // Use the public method on UnitBase
        if (target is UnitBase unit)
        {
            unit.AddShield(shieldAmount);
            Debug.Log($"{unit.gameObject.name} gained {shieldAmount} shield");
        }
    }
}