using UnityEngine;

public class MoveAction : TurnAction
{
    public string ActionName => "Move";

    public void Execute(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            if (unitBase.CanAffordAP(unitBase.GetMoveCost()))
            {
                unitBase.PreviewAPCost(unitBase.GetMoveCost());

                CameraEvent.LockCamera?.Invoke();
                CameraEvent.RecenterCamera?.Invoke();

                unit.Move();
            }
            else
            {
                unitBase.NotEnoughAP(unitBase.GetMoveCost());
            }
        }
    }
}

