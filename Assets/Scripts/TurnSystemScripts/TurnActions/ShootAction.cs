using UnityEngine;

public class ShootAction : TurnAction
{
    public string ActionName => "Shoot";

    public void Execute(Unit unit)
    {
        //Debug.Log(unit + " is shooting");
        // enter attack targeting mode
        unit.Shoot();
    }
}

