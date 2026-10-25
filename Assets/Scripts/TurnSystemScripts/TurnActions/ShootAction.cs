using UnityEngine;

public class ShootAction : TurnAction
{
    public string ActionName => "Shoot";

    public void Execute(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            if (unitBase.CanAffordAP(unitBase.GetShootCost()))
            {
                unitBase.PreviewAPCost(unitBase.GetShootCost());

                CameraEvent.LockCamera?.Invoke();
                CameraEvent.RecenterCamera?.Invoke();

                unit.Shoot();
            }
            else
            {
                unitBase.NotEnoughAP(unitBase.GetShootCost());
            }
        }
    }
}

