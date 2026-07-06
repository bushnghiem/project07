using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public ShopManager shopManager;
    public ShipHolder shipHolder;

    public Transform slotParent;
    public ShopItemSlot slotPrefab;

    public Button rerollButton;
    public Button closeButton;
    public Button buyChargesButton;
    public Button buyKeyButton;

    [Header("UI Text")]
    public TMP_Text currencyText;
    public TMP_Text keyText;
    public TMP_Text rerollText;
    public TMP_Text buyChargesText;
    public TMP_Text buyKeyText;

    private void Awake()
    {
        rerollButton.onClick.AddListener(OnRerollPressed);
        closeButton.onClick.AddListener(OnClosePressed);
        buyChargesButton.onClick.AddListener(OnBuyChargesPressed);
        buyKeyButton.onClick.AddListener(OnBuyKeyPressed);
    }

    public void PopulateShop()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (var item in shopManager.shopItems)
        {
            if (item == null) continue;

            ShopItemSlot slot = Instantiate(slotPrefab, slotParent);
            slot.Setup(item, shopManager);
        }

        RefreshTopUI();
    }

    private void RefreshTopUI()
    {
        var currency = RunManager.Instance.CurrentRun.runCurrency;
        var key = RunManager.Instance.CurrentRun.runKeys;

        currencyText.text = $"Credits: {currency}";
        keyText.text = $"Keys: {key}";
        rerollText.text = $"Reroll: {shopManager.rerollCost}";
        buyChargesText.text = $"Buy {shopManager.chargesPerPurchase} Charges: ({shopManager.chargePurchaseCost})";
        buyKeyText.text = $"Buy Key: ({shopManager.keyPurchaseCost})";
    }

    private void OnRerollPressed()
    {
        if (!RewardManager.Instance.SpendRunCurrency(shopManager.rerollCost))
        {
            Debug.Log("Too Broke");
            return;
        }

        shopManager.Reroll();
        PopulateShop();
    }

    private void OnBuyChargesPressed()
    {
        if (!RewardManager.Instance.CanAfford(shopManager.chargePurchaseCost))
            return;

        PlayerSelectionUI.Instance.Open(
            shipHolder.allPlayers,
            player =>
            {
                if (shopManager.TryPurchaseCharges(
                    player,
                    shopManager.chargePurchaseCost,
                    shopManager.chargesPerPurchase))
                {
                    RefreshTopUI();
                }
            });
    }

    private void OnBuyKeyPressed()
    {
        if (!RewardManager.Instance.CanAfford(shopManager.keyPurchaseCost))
            return;
        shopManager.TryPurchaseKey(shopManager.keyPurchaseCost);
        RefreshTopUI();
    }

    private void OnClosePressed()
    {
        gameObject.SetActive(false);

        GridUIManager.Instance.ClearState();

        FindFirstObjectByType<GridMovement>().inputLocked = false;
    }
}