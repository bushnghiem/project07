using UnityEngine;

public class StartState : BattleState
{
    public StartState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Round Started");
        manager.RoundStart();
        //manager.SwitchState(new PlayerTurnState(manager));
        manager.SwitchState(new UnitTurnState(manager));
    }
}

