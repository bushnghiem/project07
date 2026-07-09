using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class BossRewardSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public Button button;

    BossReward reward;

    public void Setup(BossReward reward, Action<BossReward> onChosen)
    {
        this.reward = reward;

        icon.sprite = reward.Icon;
        title.text = reward.Title;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onChosen(reward));
    }
}
