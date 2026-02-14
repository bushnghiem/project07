using UnityEngine;

public class StartState : BattleState
{
    public StartState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Battle Started");

        manager.SwitchState(new PlayerTurnState(manager));
    }
}

