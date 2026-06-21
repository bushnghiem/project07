
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
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

        if (shopItem.purchased)
        {
            buyButton.interactable = false;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0.5f;
            }
        }
        else
        {
            buyButton.interactable = true;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }
    }

    public void OnBuyPressed()
    {
        if (shopItem.purchased)
        {
            Debug.Log("Already bought");
            return;
        }

        if (!RewardManager.Instance.CanAfford(shopItem.price))
        {
            Debug.Log("Too broke to buy this item!");
            return;
        }

        PlayerSelectionUI.Instance.Open(
            shopManager.shipHolder.allPlayers,
            (selectedPlayer) =>
            {
                bool success = shopManager.TryPurchase(selectedPlayer, shopItem);

                if (success)
                {
                    Debug.Log($"Bought {shopItem.item.itemName} for {selectedPlayer.name}");

                    buyButton.interactable = false;

                    if (canvasGroup != null)
                    {
                        StartCoroutine(FadeTo(0.5f, 0.3f));
                    }
                }
                else
                {
                    Debug.Log($"Could not buy {shopItem.item.itemName}");
                }
            });
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsed / duration
            );

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}