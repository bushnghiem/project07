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

    public bool CanUse(UnitBase user)
    {
        return remainingCooldown <= 0 &&
               user.CurrentCharges >= itemData.chargeCost;
    }

    public bool Use(
        Unit user,
        ItemTargetData targetData
    )
    {

        UnitBase unitBase = user as UnitBase;

        if (!CanUse(unitBase))
            return false;

        if (!unitBase.SpendCharges(itemData.chargeCost))
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