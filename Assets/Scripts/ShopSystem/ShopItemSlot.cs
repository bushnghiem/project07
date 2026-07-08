using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class ShopItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;
    public CanvasGroup canvasGroup;

    private ShopItem shopItem;
    private ShopManager shopManager;

    public void Setup(ShopItem item, ShopManager manager)
    {
        shopItem = item;
        shopManager = manager;

        icon.sprite = item.item.icon;
        nameText.text = item.item.itemName;
        priceText.text = item.price.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyPressed);

        RefreshVisual();
    }

    private void RefreshVisual()
    {
        bool purchased = shopItem.purchased;

        buyButton.interactable = !purchased;

        if (canvasGroup != null)
            canvasGroup.alpha = purchased ? 0.5f : 1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (shopItem == null || shopItem.item == null)
            return;

        TooltipUI.Instance.Show(ItemTooltipBuilder.Build(shopItem));
        Debug.Log("Hover over shop item");
        Debug.Log(TooltipUI.Instance.panel.gameObject.activeSelf);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }

    public void OnBuyPressed()
    {
        if (shopItem.purchased)
            return;

        if (!RewardManager.Instance.CanAfford(shopItem.price))
            return;

        PlayerSelectionUI.Instance.Open(
            shopManager.shipHolder.allPlayers,
            (selectedPlayer) =>
            {
                bool success = shopManager.TryPurchase(selectedPlayer, shopItem);

                if (success)
                {
                    RefreshVisual();
                }
            });
    }
}