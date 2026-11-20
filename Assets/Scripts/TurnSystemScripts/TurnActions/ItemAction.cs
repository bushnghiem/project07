using UnityEngine;

public class ItemAction : TurnAction
{
    private ItemTargetingController targetingController;

    public string ActionName => "Item";

    public ItemAction(ItemTargetingController controller)
    {
        if (controller == null)
        {
            Debug.LogError(
                "ItemAction created with NULL targeting controller"
            );
        }

        targetingController = controller;
    }

    public void Execute(Unit unit)
    {
        if (unit == null)
            return;

        UnitBase unitBase = unit as UnitBase;

        if (unitBase == null)
            return;

        ActiveItemInstance item = unitBase.GetActiveItem();

        if (item == null)
        {
            CombatTextManager.Instance?.ShowNoActiveItem(
                unitBase.transform.position
            );

            return;
        }

        if (targetingController == null)
        {
            Debug.LogError(
                "ItemAction: targetingController not assigned"
            );

            return;
        }

        if (item.IsOnCooldown)
        {
            CombatTextManager.Instance?.ShowItemOnCooldown(
                unitBase.transform.position
            );

            return;
        }

        if (!item.HasEnoughCharges(unitBase))
        {
            CombatTextManager.Instance?.ShowNotEnoughCharges(
                unitBase.transform.position
            );

            return;
        }

        int cost = unitBase.GetItemCost();

        if (!unitBase.CanAffordAP(cost))
        {
            unitBase.NotEnoughAPForAction(
                cost,
                "Use Item"
            );

            return;
        }

        unitBase.PreviewAPCost(cost);

        targetingController.BeginTargeting(
            unitBase,
            item
        );
    }
}