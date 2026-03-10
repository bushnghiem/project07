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

    private Player selectedPlayer;

    private void Awake()
    {
        rerollButton.onClick.AddListener(OnRerollPressed);
        closeButton.onClick.AddListener(OnClosePressed);
    }

    public void SetSelectedPlayer(Player player)
    {
        selectedPlayer = player;
    }

    public void PopulateShop()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (var item in shopManager.shopItems)
        {
            ShopItemSlot slot = Instantiate(slotPrefab, slotParent);
            slot.Setup(item, shopManager, selectedPlayer);
        }
    }

    private void OnRerollPressed()
    {
        if (!RewardManager.Instance.SpendRunCurrency(shopManager.rerollCost))
        {
            Debug.Log("Too Broke");
            return;
        }

        Debug.Log("Reroll");
        shopManager.Reroll();
        PopulateShop();
    }

    private void OnClosePressed()
    {
        gameObject.SetActive(false);
    }
}
