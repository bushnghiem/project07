using UnityEngine;

public class ItemAction : TurnAction
{
    public string ActionName => "Item";

    public void Execute(Unit unit)
    {
        //Debug.Log(unit + " is moving");
        // enter movement targeting mode
        unit.Item();
    }
}
