using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : BattleState
{
    public WinState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        RunManager.Instance.CurrentRun.currentFloorData.clearedCombatTiles.Add(RunManager.Instance.CurrentRun.currentFloorData.currentGridPosition);
        EncounterData fightData = RunManager.Instance.CurrentRun.currentFloorData.currentEncounter;
        RewardManager.Instance.AddRunCurrency(fightData.runCurrencyReward);
        RewardManager.Instance.AddMetaCurrency(fightData.metaCurrencyReward);
        SaveManager.Instance.SaveRun();
        SaveManager.Instance.SaveMeta();
        //TurnEvent.OnFightWon?.Invoke();
        SceneManager.LoadScene("TestGrid");
        Debug.Log("You Win!");
    }
}

public class LoseState : BattleState
{
    public LoseState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        //TurnEvent.OnFightLost?.Invoke();
        SaveManager.Instance.DeleteRun();
        SaveManager.Instance.SaveMeta();
        SceneManager.LoadScene("TestMainMenu");
        Debug.Log("You Lose!");
    }
}

