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

    [Header("UI Text")]
    public TMP_Text currencyText;
    public TMP_Text rerollText;

    private void Awake()
    {
        rerollButton.onClick.AddListener(OnRerollPressed);
        closeButton.onClick.AddListener(OnClosePressed);
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

        currencyText.text = $"Credits: {currency}";
        rerollText.text = $"Reroll: {shopManager.rerollCost}";
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

    private void OnClosePressed()
    {
        gameObject.SetActive(false);

        GridUIManager.Instance.ClearState();

        FindFirstObjectByType<GridMovement>().inputLocked = false;
    }
}