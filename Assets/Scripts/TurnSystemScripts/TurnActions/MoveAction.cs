using UnityEngine;

public class MoveAction : TurnAction
{
    public string ActionName => "Move";

    public void Execute(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            int cost = unitBase.GetMoveCost();

            if (unitBase.CanAffordAP(cost))
            {
                unitBase.PreviewAPCost(cost);

                CameraEvent.LockCamera?.Invoke();
                CameraEvent.RecenterCamera?.Invoke();

                unit.Move();
            }
            else
            {
                unitBase.NotEnoughAPForAction(cost, "Move");
            }
        }
    }
}