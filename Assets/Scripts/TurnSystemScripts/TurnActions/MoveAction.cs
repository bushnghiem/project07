using UnityEngine;

public class MoveAction : TurnAction
{
    public string ActionName => "Move";

    public void Execute(Unit unit)
    {
        CameraEvent.LockCamera?.Invoke();
        CameraEvent.RecenterCamera?.Invoke();

        unit.Move();
    }
}

