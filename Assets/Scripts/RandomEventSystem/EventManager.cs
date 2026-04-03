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
        Vector2Int pos = run.currentFloorData.currentGridPosition;

        foreach (var outcome in option.outcomes)
        {
            ApplyOutcome(outcome);
        }

        SaveManager.Instance.SaveRun();
    }

    void ApplyOutcome(EventOutcome outcome)
    {
        var run = RunManager.Instance.CurrentRun;
        Vector2Int pos = RunManager.Instance.CurrentRun.currentFloorData.currentGridPosition;

        Debug.Log("Instance: " + PlayerSelectionUI.Instance);
        Debug.Log("ShipHolder: " + shipHolder);
        Debug.Log("Players list: " + (shipHolder != null ? shipHolder.allPlayers : null));

        if (outcome.tileModification == TileModification.Clear && !run.currentFloorData.clearedEventTiles.Contains(pos))
        {
            run.currentFloorData.clearedEventTiles.Add(pos);
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

            case OutcomeType.StartCombat:
                run.currentFloorData.currentEncounter = outcome.encounter;
                SceneManager.LoadScene("SpawnTestScene");
                break;

            case OutcomeType.HealPlayer:
                PlayerSelectionUI.Instance.Open(
                    shipHolder.allPlayers,
                    (player) =>
                    {
                        RewardManager.Instance.HealPlayer(player, outcome.value);
                    });
                break;

            case OutcomeType.DamagePlayer:
                PlayerSelectionUI.Instance.Open(
                    shipHolder.allPlayers,
                    (player) =>
                    {
                        RewardManager.Instance.HurtPlayer(player, outcome.value);
                    });
                break;

            case OutcomeType.GiveItem:
                PlayerSelectionUI.Instance.Open(
                    shipHolder.allPlayers,
                    (player) =>
                    {
                        RewardManager.Instance.AddItemToPlayer(player, outcome.item);
                    });
                break;

            case OutcomeType.Nothing:
                break;
        }
    }
}
