using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : BattleState
{
    public WinState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;
        EncounterData fightData = floor.currentEncounter;

        RewardManager.Instance.AddRunCurrency(fightData.runCurrencyReward);
        RewardManager.Instance.AddMetaCurrency(fightData.metaCurrencyReward);

        switch (fightData.encounterType)
        {
            case EncounterType.Normal:

                if (floor.currentEncounterIsCorrupted)
                {
                    floor.clearedCorruptionTiles.Add(
                        floor.currentGridPosition);

                    Debug.Log("Corruption encounter cleared.");
                }
                else
                {
                    floor.clearedCombatTiles.Add(
                        floor.currentGridPosition);

                    Debug.Log("Normal encounter cleared.");
                }

                break;

            case EncounterType.Elite:

                if (floor.currentEncounterIsCorrupted)
                {
                    floor.clearedCorruptionTiles.Add(
                        floor.currentGridPosition);

                    Debug.Log("Corruption elite cleared.");
                }
                else
                {
                    floor.clearedCombatTiles.Add(
                        floor.currentGridPosition);

                    Debug.Log("Elite encounter cleared.");
                }

                break;

            case EncounterType.Boss:

                floor.bossDefeated = true;

                Debug.Log("Boss defeated!");

                break;
        }

        floor.currentEncounter = null;
        floor.currentEncounterIsCorrupted = false;

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

