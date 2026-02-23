using UnityEngine;

[System.Serializable]
public class ActiveItemInstance
{
    public ActiveItem itemData;        // Runtime reference
    private int remainingCooldown = 0; // Tracks cooldown in runtime

    // Constructor for runtime usage
    public ActiveItemInstance(ActiveItem data)
    {
        itemData = data;
    }

    // Check if item can be used
    public bool CanUse()
    {
        return remainingCooldown <= 0;
    }

    // Use the item
    public bool Use(Unit user, Unit target)
    {
        if (!CanUse())
            return false;

        itemData.Activate(user, target);
        remainingCooldown = itemData.cooldownTurns;
        return true;
    }

    // Called at start of turn
    public void OnTurnStart()
    {
        if (remainingCooldown > 0)
            remainingCooldown--;
    }

    public int GetRemainingCooldown() => remainingCooldown;
}