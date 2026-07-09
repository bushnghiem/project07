using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemChoiceSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public Button button;

    Item item;

    [SerializeField] private TooltipTrigger tooltipTrigger;

    public void Setup(Item newItem, System.Action<Item> onChosen)
    {
        item = newItem;

        icon.sprite = item.icon;
        nameText.text = item.itemName;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onChosen(item));

        tooltipTrigger.SetProvider(() => ItemTooltipBuilder.Build(item));
    }
}