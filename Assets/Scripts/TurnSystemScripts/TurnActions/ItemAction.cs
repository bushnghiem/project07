using UnityEngine;

public class ItemAction : TurnAction
{
    private ItemTargetingController targetingController;

    public string ActionName => "Item";

    public ItemAction(ItemTargetingController controller)
    {
        if (controller == null)
        {
            Debug.LogError("ItemAction created with NULL targeting controller");
        }

        targetingController = controller;
    }

    public void Execute(Unit unit)
    {
        if (unit == null)
        {
            Debug.LogError("ItemAction: unit is null");
            return;
        }

        UnitBase unitBase = unit as UnitBase;

        if (unitBase == null)
        {
            Debug.LogError("ItemAction: unit is not UnitBase");
            return;
        }

        ActiveItemInstance item = unitBase.GetActiveItem();

        if (item == null)
        {
            Debug.LogError("ItemAction: no active item equipped");
            return;
        }

        if (targetingController == null)
        {
            Debug.LogError("ItemAction: targetingController not assigned");
            return;
        }

        targetingController.BeginTargeting(unitBase, item);
    }
}
