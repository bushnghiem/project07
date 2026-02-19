using UnityEngine;

public class UnitTurnState : BattleState
{
    public UnitTurnState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log(manager.currentUnit +  "'s Turn");
        manager.currentUnit.StartTurn();
        //TurnEvent.OnUnitTurnStart?.Invoke(manager.currentUnit);
    }

    public override void Exit()
    {
        
    }
}
