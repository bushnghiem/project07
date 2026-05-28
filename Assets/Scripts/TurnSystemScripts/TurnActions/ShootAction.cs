using UnityEngine;

public class ShootAction : TurnAction
{
    public string ActionName => "Shoot";

    public void Execute(Unit unit)
    {
        CameraEvent.LockCamera?.Invoke();
        CameraEvent.RecenterCamera?.Invoke();

        unit.Shoot();
    }
}

