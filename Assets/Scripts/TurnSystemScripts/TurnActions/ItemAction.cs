using UnityEngine;

public class ItemAction : TurnAction
{
    public string ActionName => "Item";

    public void Execute(Unit unit)
    {
        unit.Item();
    }
}
