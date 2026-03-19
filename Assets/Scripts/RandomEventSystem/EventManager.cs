using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public GridManager gridManager;
    public ShipHolder shipHolder;

    private void Awake()
    {
        Instance = this;
    }

    public void ExecuteOption(EventOption option)
    {
        var run = RunManager.Instance.CurrentRun;
        Vector2Int pos = run.currentGridPosition;

        foreach (var outcome in option.outcomes)
        {
            ApplyOutcome(outcome);
        }

        SaveManager.Instance.SaveRun();
    }

    void ApplyOutcome(EventOutcome outcome)
    {
        var run = RunManager.Instance.CurrentRun;
        Vector2Int pos = RunManager.Instance.CurrentRun.currentGridPosition;

        if (outcome.tileModification == TileModification.Clear && !run.clearedEventTiles.Contains(pos))
        {
            run.clearedEventTiles.Add(pos);
            gridManager.clearEventTile(pos.x, pos.y);
            gridManager.ClearEventVisualAt(pos.x, pos.y);
        }

        switch (outcome.type)
        {
            case OutcomeType.GainCurrency:
                RewardManager.Instance.AddRunCurrency(outcome.value);
                break;

            case OutcomeType.LoseCurrency:
                RewardManager.Instance.SpendRunCurrency(outcome.value);
                break;

            case OutcomeType.HealPlayer:
                RewardManager.Instance.HealAllPlayers(outcome.value);
                break;

            case OutcomeType.DamagePlayer:
                RewardManager.Instance.DamageAllPlayers(outcome.value);
                break;

            case OutcomeType.StartCombat:
                run.currentEncounter = outcome.encounter;
                SceneManager.LoadScene("SpawnTestScene");
                break;

            case OutcomeType.GiveItem:
                RewardManager.Instance.AddItemToAllPlayers(outcome.item);
                break;

            case OutcomeType.Nothing:
                break;
        }
    }
}
