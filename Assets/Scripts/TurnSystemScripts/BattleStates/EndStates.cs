using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : BattleState
{
    public WinState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        TurnEvent.OnFightWon?.Invoke();
        SceneManager.LoadScene("SpawnTestScene 1");
        Debug.Log("You Win!");
        
    }
}

public class LoseState : BattleState
{
    public LoseState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        TurnEvent.OnFightLost?.Invoke();
        SceneManager.LoadScene("TestMainMenu");
        Debug.Log("You Lose!");
    }
}

