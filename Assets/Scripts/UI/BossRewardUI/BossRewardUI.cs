using UnityEngine;
using System;
using System.Collections.Generic;

public class BossRewardUI : MonoBehaviour
{
    public static BossRewardUI Instance;

    [Header("References")]
    public Transform rewardParent;
    public BossRewardSlot rewardSlotPrefab;

    private List<BossReward> rewards;
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

        gameObject.SetActive(false);
    }

    public void Show(List<BossReward> rewards, Action onFinished)
    {
        GridUIManager.Instance.SetState(UIState.BossReward);

        gameObject.SetActive(true);

        this.rewards = rewards;
        this.onFinished = onFinished;

        PopulateSlots();
    }

    public void SelectReward(BossReward reward)
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

        foreach (BossReward reward in rewards)
        {
            BossRewardSlot slot =
                Instantiate(rewardSlotPrefab, rewardParent);

            slot.Setup(reward, SelectReward);
        }
    }
}