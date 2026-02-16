using UnityEngine;

public class ActiveItemInstance
{
    public ActiveItem itemData;

    private int remainingCooldown = 0;

    public ActiveItemInstance(ActiveItem data)
    {
        itemData = data;
    }

    public bool CanUse()
    {
        return remainingCooldown <= 0;
    }

    public void Use(Unit user, Unit target)
    {
        if (!CanUse()) return;

        itemData.Activate(user, target);

        remainingCooldown = itemData.cooldownTurns;
    }

    public void OnTurnStart()
    {
        if (remainingCooldown > 0)
            remainingCooldown--;
    }

    public int GetRemainingCooldown() => remainingCooldown;
}
