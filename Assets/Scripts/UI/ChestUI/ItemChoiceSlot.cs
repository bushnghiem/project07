using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemChoiceSlot :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public Image icon;
    public TMP_Text nameText;
    public Button button;

    Item item;

    public void Setup(Item newItem, System.Action<Item> onChosen)
    {
        item = newItem;

        icon.sprite = item.icon;
        nameText.text = item.itemName;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onChosen(item));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.Instance.Show(
            ItemTooltipBuilder.Build(item));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }
}