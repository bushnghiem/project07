using UnityEngine;

public class ShootAction : TurnAction
{
    public string ActionName => "Shoot";

    public void Execute(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            int cost = unitBase.GetShootCost();

            if (unitBase.CanAffordAP(cost))
            {
                unitBase.PreviewAPCost(cost);

                CameraEvent.LockCamera?.Invoke();
                CameraEvent.RecenterCamera?.Invoke();

                unit.Shoot();
            }
            else
            {
                unitBase.NotEnoughAPForAction(cost, "Shoot");
            }
        }
    }
}