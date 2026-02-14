using UnityEngine;

public class PlayerTurnState : BattleState
{
    public PlayerTurnState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log("Player Turn");
        TurnEvent.OnPlayerTurnStart?.Invoke(manager.player);
    }

    public override void Exit()
    {
        manager.player.EndOfTurn();
    }
}
