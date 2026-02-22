using UnityEngine;

public class WinState : BattleState
{
    public WinState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        TurnEvent.OnFightWon?.Invoke();
        Debug.Log("You Win!");
    }
}

public class LoseState : BattleState
{
    public LoseState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        TurnEvent.OnFightLost?.Invoke();
        Debug.Log("You Lose!");
    }
}

