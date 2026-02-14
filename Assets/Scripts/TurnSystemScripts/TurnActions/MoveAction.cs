using UnityEngine;

public class MoveAction : TurnAction
{
    public string ActionName => "Move";

    public void Execute(Unit unit)
    {
        //Debug.Log(unit + " is moving");
        // enter movement targeting mode
        unit.Move();
    }
}

