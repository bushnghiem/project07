using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardMenuUI : MonoBehaviour
{
    public static RewardMenuUI Instance;
    public Button closeButton;

    [Header("References")]
    public Transform rewardParent;
    public RewardSlot rewardSlotPrefab;

    private List<Reward> rewards;
    private Action onFinished;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        closeButton.onClick.AddListener(OnClosePressed);
        gameObject.SetActive(false);
    }

    public void Show(List<Reward> rewards, Action onFinished)
    {
        GridUIManager.Instance.SetState(UIState.BossReward);

        gameObject.SetActive(true);

        this.rewards = rewards;
        this.onFinished = onFinished;

        PopulateSlots();
    }

    public void SelectReward(Reward reward)
    {
        reward.Claim();
    }

    public void FinishReward()
    {
        foreach (Transform child in rewardParent)
            Destroy(child.gameObject);

        gameObject.SetActive(false);

        GridUIManager.Instance.ClearState();

        onFinished?.Invoke();
    }

    private void PopulateSlots()
    {
        foreach (Transform child in rewardParent)
            Destroy(child.gameObject);

        foreach (Reward reward in rewards)
        {
            RewardSlot slot =
                Instantiate(rewardSlotPrefab, rewardParent);

            slot.Setup(reward, SelectReward);
        }
    }

    private void OnClosePressed()
    {
        gameObject.SetActive(false);

        GridUIManager.Instance.ClearState();

        onFinished?.Invoke();
    }
}