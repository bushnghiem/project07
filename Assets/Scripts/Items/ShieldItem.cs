using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Shield")]
public class ShieldItem : ActiveItem
{
    public int shieldAmount = 1;

    public override void Activate(Unit user, Unit target)
    {
        if (target == null) return;

        if (target.IsPlayerControllable)
        {
            Player player = (Player)target;
            player.healthComp.addShield(shieldAmount);
            Debug.Log(target + " gained shield");
        }
        else
        {
            Enemy enemy = (Enemy)target;
            enemy.healthComp.addShield(shieldAmount);
            Debug.Log(target + " gained shield");
        }
    }
}