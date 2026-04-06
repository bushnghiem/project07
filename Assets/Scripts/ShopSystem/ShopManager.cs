using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public ShipHolder shipHolder;

    public int shopItemCount = 3;
    public int rerollCost = 50;

    public List<ShopItem> shopItems = new();
    public Vector2Int currentShopPosition;

    public void GenerateShop(Vector2Int shopPosition, bool forceReroll = false)
    {
        currentShopPosition = shopPosition;

        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        ShopData shopData = floor.shops.Find(s => s.gridPosition == shopPosition);

        if (shopData != null && !forceReroll)
        {
            shopItems = shopData.shopItems;
            return;
        }

        if (shopData == null)
        {
            shopData = new ShopData { gridPosition = shopPosition };
            floor.shops.Add(shopData);
        }
        else if (forceReroll)
        {
            shopData.rerollCount++;
        }

        int seed = run.runSeed
                   ^ (shopPosition.x * 73856093)
                   ^ (shopPosition.y * 19349663)
                   ^ (shopData.rerollCount * 19349669);

        System.Random rng = new System.Random(seed);

        shopItems = new List<ShopItem>();

        var allItems = floor.contentProfile.shopItems;

        for (int i = 0; i < shopItemCount; i++)
        {
            int index = rng.Next(0, allItems.Count);
            Item item = allItems[index];

            shopItems.Add(new ShopItem
            {
                item = item,
                price = item.price,
                purchased = false
            });
        }

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
        /*
        var run = RunManager.Instance.CurrentRun;
        var shopData = run.shops.Find(s => s.gridPosition == currentShopPosition);

        if (shopData == null)
        {
            Debug.LogError("ShopData not found!");
            return false;
        }

        var savedItem = shopData.shopItems.Find(i => i.item == shopItem.item);

        if (savedItem != null)
        {
            savedItem.purchased = true;
        }
        */
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