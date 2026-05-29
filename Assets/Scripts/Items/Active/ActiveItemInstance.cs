using UnityEngine;

[System.Serializable]
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

    public bool Use(
        Unit user,
        ItemTargetData targetData
    )
    {
        if (!CanUse())
            return false;

        itemData.Execute(user, targetData);

        remainingCooldown =
            itemData.cooldownTurns;

        return true;
    }

    public void OnTurnStart()
    {
        if (remainingCooldown > 0)
            remainingCooldown--;
    }

    public int GetRemainingCooldown()
    {
        return remainingCooldown;
    }
}