using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : BattleState
{
    public WinState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;
        EncounterData fightData = floor.currentEncounter;

        // Rewards
        RewardManager.Instance.AddRunCurrency(fightData.runCurrencyReward);
        RewardManager.Instance.AddMetaCurrency(fightData.metaCurrencyReward);

        // Handle encounter completion
        switch (fightData.encounterType)
        {
            case EncounterType.Normal:

                floor.clearedCombatTiles.Add(floor.currentGridPosition);

                Debug.Log("Normal encounter cleared.");

                break;

            case EncounterType.Elite:

                floor.clearedCombatTiles.Add(floor.currentGridPosition);

                Debug.Log("Elite encounter cleared.");

                break;

            case EncounterType.Boss:

                floor.bossDefeated = true;

                Debug.Log("Boss defeated!");

                break;
        }

        // Cleanup current encounter
        //floor.currentEncounter = null;

        SaveManager.Instance.SaveRun();
        SaveManager.Instance.SaveMeta();

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
        SceneManager.LoadScene("MainMenu");
        Debug.Log("You Lose!");
    }
}

