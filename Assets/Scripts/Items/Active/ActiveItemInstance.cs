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

    public bool IsOnCooldown => remainingCooldown > 0;

    public bool HasEnoughCharges(UnitBase user)
    {
        if (user == null || itemData == null)
            return false;

        return user.CurrentCharges >= itemData.chargeCost;
    }

    public bool HasEnoughAP(UnitBase user)
    {
        if (user == null || itemData == null)
            return false;

        return user.CanAffordAP(
            Mathf.Max(1, itemData.apCost)
        );
    }

    public bool CanUse(UnitBase user)
    {
        if (user == null || itemData == null)
            return false;

        return !IsOnCooldown &&
               HasEnoughCharges(user) &&
               HasEnoughAP(user);
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