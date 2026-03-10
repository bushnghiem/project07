using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;

    private ShopItem shopItem;
    private ShopManager shopManager;
    private Player player;

    public void Setup(ShopItem item, ShopManager manager, Player p)
    {
        shopItem = item;
        shopManager = manager;
        player = p;

        icon.sprite = item.item.icon;
        nameText.text = item.item.itemName;
        priceText.text = item.price.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyPressed);
    }

    public void OnBuyPressed()
    {
        bool success = shopManager.TryPurchase(player, shopItem);

        if (success)
            Debug.Log($"Bought {shopItem.item.itemName}!");
        else
            Debug.Log($"Could not buy {shopItem.item.itemName}");
    }
}