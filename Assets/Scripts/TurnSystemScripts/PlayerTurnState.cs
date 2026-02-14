using UnityEngine;

public class PlayerTurnState : BattleState
{
    public PlayerTurnState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Player Turn");
        manager.player.EnablePlayer(true);
    }

    public override void Exit()
    {
        manager.player.EnablePlayer(false);
    }
}
