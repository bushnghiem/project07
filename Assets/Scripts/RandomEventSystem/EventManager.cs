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

    bool RequiresPlayerSelection(EventOutcome outcome)
    {
        switch (outcome.type)
        {
            case OutcomeType.HealPlayer:
            case OutcomeType.DamagePlayer:
            case OutcomeType.GiveItem:
            case OutcomeType.GiveCharges:
                return true;

            default:
                return false;
        }
    }

    void ApplyOutcomeToPlayer(EventOutcome outcome, Player player)
    {
        switch (outcome.type)
        {
            case OutcomeType.HealPlayer:
                RewardManager.Instance.HealPlayer(
                    player,
                    outcome.value
                );
                break;

            case OutcomeType.DamagePlayer:
                if (outcome.damage == null)
                {
                    Debug.LogWarning(
                        "DamagePlayer outcome missing DamageDefinition!"
                    );
                    return;
                }

                DamageInfo damageInfo = outcome.damage.ToDamageInfo();

                RewardManager.Instance.HurtPlayer(
                    player,
                    damageInfo
                );
                break;

            case OutcomeType.GiveItem:
                RewardManager.Instance.AddItemToPlayer(
                    player,
                    outcome.item
                );
                break;

            case OutcomeType.GiveCharges:
                RewardManager.Instance.GivePlayerCharges(
                    player,
                    outcome.value
                );
                break;
        }
    }

    void ApplyOutcome(EventOutcome outcome)
    {
        var run = RunManager.Instance.CurrentRun;
        Vector2Int pos =
            run.currentFloorData.currentGridPosition;

        if (outcome.tileModification == TileModification.Clear)
        {
            gridManager.ClearEventTile(pos.x, pos.y);
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

            case OutcomeType.TakeTime:
                AddTime(outcome.value);
                break;

            case OutcomeType.StartQuest:
                QuestManager.Instance.StartQuest(outcome.quest);
                break;

            case OutcomeType.Nothing:
                break;

            // Player-targeted outcomes are handled separately
            // by ApplyOutcomeToPlayer().
            case OutcomeType.HealPlayer:
            case OutcomeType.DamagePlayer:
            case OutcomeType.GiveItem:
            case OutcomeType.GiveCharges:
                Debug.LogWarning(
                    $"Player-targeted outcome {outcome.type} " +
                    "was passed to ApplyOutcome instead of ApplyOutcomeToPlayer."
                );
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
        List<EventOutcome> playerOutcomes = new List<EventOutcome>();

        foreach (var group in option.outcomeGroups)
        {
            if (!Roll(group.groupChance, rng))
                continue;

            foreach (var outcome in group.outcomes)
            {
                if (!Roll(outcome.chance, rng))
                    continue;

                if (RequiresPlayerSelection(outcome))
                {
                    playerOutcomes.Add(outcome);
                }
                else
                {
                    ApplyOutcome(outcome);
                }
            }
        }

        // If we have any outcomes that require a player,
        // ask for the player only once.
        if (playerOutcomes.Count > 0)
        {
            PlayerSelectionUI.Instance.Open(
                shipHolder.allPlayers,
                (player) =>
                {
                    foreach (var outcome in playerOutcomes)
                    {
                        ApplyOutcomeToPlayer(outcome, player);
                    }
                });
        }
    }

}
