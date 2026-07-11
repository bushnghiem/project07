using UnityEngine;
using System;
using System.Collections.Generic;

public enum UIState
{
    None,
    Grid,
    Event,
    Shop,
    Chest,
    Fleet,
    BossReward
}

public class GridUIManager : MonoBehaviour
{
    public static GridUIManager Instance;

    public UIState CurrentState { get; private set; } = UIState.None;

    [Header("UI References")]
    public FleetUI fleetUI;
    public ShopUI shopUI;
    public EventUI eventUI;
    public ChestUI chestUI;
    public RewardMenuUI rewardMenuUI;
    public TooltipUI tooltipUI;
    public PlayerSelectionUI playerSelectionUI;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanOpen(UIState state)
    {
        return CurrentState == UIState.None || CurrentState == state;
    }

    public void SetState(UIState newState)
    {
        CurrentState = newState;

        if (tooltipUI != null)
            tooltipUI.Hide();

        // Close conflicting UI
        if (newState != UIState.Fleet && fleetUI != null)
            fleetUI.Close();

        if (newState != UIState.Shop && shopUI != null)
            shopUI.gameObject.SetActive(false);

        if (newState != UIState.Event && eventUI != null)
            eventUI.gameObject.SetActive(false);

        if (newState != UIState.Chest && chestUI != null)
            chestUI.gameObject.SetActive(false);

        if (newState != UIState.BossReward && rewardMenuUI != null)
            rewardMenuUI.gameObject.SetActive(false);
    }

    public void ClearState()
    {
        CurrentState = UIState.None;

        if (tooltipUI != null)
            tooltipUI.Hide();
    }

    public void OpenChest()
    {
        SetState(UIState.Chest);

        if (chestUI != null)
        {
            chestUI.Show();
        }
    }

    public void OpenBossReward(List<Reward> rewards, Action onFinished)
    {
        SetState(UIState.BossReward);

        if (rewardMenuUI != null)
        {
            rewardMenuUI.Show(rewards, onFinished);
        }
    }

    public void OpenPlayerSelection(List<Player> players, Action<Player> callback)
    {
        tooltipUI?.LockWorldTooltips();

        if (playerSelectionUI != null)
            playerSelectionUI.Open(players, callback);
    }

    public void ClosePlayerSelection()
    {
        tooltipUI?.UnlockWorldTooltips();

        if (playerSelectionUI != null)
            playerSelectionUI.gameObject.SetActive(false);
    }
}