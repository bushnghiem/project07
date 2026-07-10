using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BossRewardSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public Button button;

    [SerializeField] private TooltipTrigger tooltipTrigger;

    private BossReward reward;

    public void Setup(BossReward reward, Action<BossReward> onChosen)
    {
        this.reward = reward;

        icon.sprite = reward.Icon;
        title.text = reward.Title;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onChosen(reward));

        // Hook into the game's tooltip system.
        if (tooltipTrigger != null)
        {
            tooltipTrigger.SetProvider(() => BossRewardTooltipBuilder.Build(reward));
        }
    }
}