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
        var pos = run.currentFloorData.currentGridPosition;
        var floor = run.currentFloorData;

        int eventSeed =
            run.runSeed ^
            (pos.x * 73856093) ^
            (pos.y * 19349663) ^
            floor.timeElapsed;

        System.Random eventRng = new System.Random(eventSeed);

        ResolveOption(option, eventRng);

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

            case OutcomeType.GainKeys:
                RewardManager.Instance.AddRunKeys(outcome.value);
                break;

            case OutcomeType.LoseKeys:
                RewardManager.Instance.SpendRunKeys(outcome.value);
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
                        if (outcome.damage == null)
                        {
                            Debug.LogWarning("DamagePlayer outcome missing DamageDefinition!");
                            return;
                        }
                        DamageInfo damageInfo = outcome.damage.ToDamageInfo();
                        RewardManager.Instance.HurtPlayer(player, damageInfo);
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

            case OutcomeType.GiveCharges:
                PlayerSelectionUI.Instance.Open(
                    shipHolder.allPlayers,
                    (player) =>
                    {
                        RewardManager.Instance.GivePlayerCharges(player, outcome.value);
                    });
                break;

            case OutcomeType.TakeTime:
                AddTime(outcome.value);
                break;

            case OutcomeType.Nothing:
                break;
        }
    }

    void AddTime(int amount)
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        floor.timeElapsed += amount;

        CorruptionManager.Instance.OnTimePassed();

        SaveManager.Instance.SaveRun();
    }

    bool Roll(float chance, System.Random rng)
    {
        return rng.NextDouble() <= chance;
    }

    void ResolveOption(EventOption option, System.Random rng)
    {
        foreach (var group in option.outcomeGroups)
        {
            if (!Roll(group.groupChance, rng))
                continue;

            foreach (var outcome in group.outcomes)
            {
                if (Roll(outcome.chance, rng))
                {
                    ApplyOutcome(outcome);
                }
            }
        }
    }
}
