using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemChoiceSlot : MonoBehaviour
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
}