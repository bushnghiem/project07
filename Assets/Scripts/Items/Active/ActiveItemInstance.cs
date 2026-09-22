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
        if (user == null || itemData == null)
            return false;

        return remainingCooldown <= 0 &&
               user.CurrentCharges >= itemData.chargeCost &&
               user.CurrentAP >= itemData.apCost;
    }


    public bool Use(
        Unit user,
        ItemTargetData targetData,
        ActionContext context
    )
    {

        UnitBase unitBase = user as UnitBase;

        if (!CanUse(unitBase))
            return false;

        if (!unitBase.SpendCharges(itemData.chargeCost))
            return false;

        itemData.Execute(user, targetData, context);

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