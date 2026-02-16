using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Heal")]
public class HealItem : ActiveItem
{
    public int healAmount = 100;

    public override void Activate(Unit user, Unit target)
    {
        if (target == null) return;

        Debug.Log(target + " healed 100");
        target.Heal(healAmount);
    }
}
