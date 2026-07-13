using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RewardSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public Button button;

    [SerializeField] private TooltipTrigger tooltipTrigger;

    private Reward reward;

    public void Setup(Reward reward, Action<Reward> onChosen)
    {
        this.reward = reward;

        icon.sprite = reward.Icon;
        title.text = reward.Title;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onChosen(reward));

        // Hook into the game's tooltip system.
        if (tooltipTrigger != null)
        {
            tooltipTrigger.SetProvider(() => RewardTooltipBuilder.Build(reward));
        }
    }
}