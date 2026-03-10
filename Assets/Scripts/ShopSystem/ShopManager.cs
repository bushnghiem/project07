using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;

    public int shopItemCount = 3;

    public int rerollCost = 50;

    public List<ShopItem> shopItems = new();
    public Vector2Int currentShopPosition;

    public void GenerateShop(Vector2Int shopPosition, bool forceReroll = false)
    {
        currentShopPosition = shopPosition;

        var run = RunManager.Instance.CurrentRun;
        ShopData shopData = run.shops.Find(s => s.gridPosition == shopPosition);

        if (shopData != null && !forceReroll)
        {
            // Load existing shop
            shopItems = shopData.shopItems;
            return;
        }

        // If no shop data exists, create it
        if (shopData == null)
        {
            shopData = new ShopData { gridPosition = shopPosition };
            run.shops.Add(shopData);
        }
        else if (forceReroll)
        {
            shopData.rerollCount++; // increment reroll for RNG
        }

        // Use rerollCount in RNG so each reroll produces different items
        int seed = run.runSeed
                   ^ (shopPosition.x * 73856093)
                   ^ (shopPosition.y * 19349663)
                   ^ (shopData.rerollCount * 19349669);

        System.Random rng = new System.Random(seed);

        shopItems = new List<ShopItem>();
        var allItems = itemDatabase.GetAllItems();

        for (int i = 0; i < shopItemCount; i++)
        {
            int index = rng.Next(0, allItems.Count);
            Item item = allItems[index];

            ShopItem shopItem = new ShopItem
            {
                item = item,
                price = item.price,
                purchased = false
            };

            shopItems.Add(shopItem);
        }

        // Save generated shop items
        shopData.shopItems = shopItems;
    }

    public bool TryPurchase(Player player, ShopItem shopItem)
    {
        if (shopItem.purchased)
        {
            Debug.Log("Already Bought");
            return false;
        }

        bool spent = RewardManager.Instance.SpendRunCurrency(shopItem.price);

        if (!spent)
        {
            Debug.Log("Too broke");
            return false;
        }

        shopItem.purchased = true;

        shopItem.item.OnAcquire(player);

        player.AddItemToRunData(shopItem.item);
        player.RefreshItemDebug();

        return true;
    }

    public void Reroll()
    {
        GenerateShop(currentShopPosition, forceReroll: true);
    }
}
