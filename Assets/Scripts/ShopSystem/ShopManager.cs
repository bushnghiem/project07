using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public ShipHolder shipHolder;

    public int shopItemCount = 3;
    public int rerollCost = 50;
    public int chargePurchaseCost = 30;
    public int chargesPerPurchase = 3;

    public List<ShopItem> shopItems = new();
    public Vector2Int currentShopPosition;

    public void GenerateShop(Vector2Int shopPosition, bool forceReroll = false)
    {
        currentShopPosition = shopPosition;

        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        ShopData shopData = floor.shops.Find(s => s.gridPosition == shopPosition);

        if (shopData == null)
        {
            shopData = new ShopData { gridPosition = shopPosition };
            floor.shops.Add(shopData);
        }

        if (shopData.shopItems != null && shopData.shopItems.Count == 3 && !forceReroll)
        {
            shopItems = shopData.shopItems;
            return;
        }

        if (forceReroll)
            shopData.rerollCount++;

        int seed = run.runSeed
                   ^ (shopPosition.x * 73856093)
                   ^ (shopPosition.y * 19349663)
                   ^ (shopData.rerollCount * 19349669);

        System.Random rng = new System.Random(seed);

        var allItems = floor.contentProfile.shopItems;

        var passiveItems = allItems.Where(i => i.slotType == ItemSlotType.Passive).ToList();
        var activeItems = allItems.Where(i => i.slotType == ItemSlotType.Active).ToList();
        var projectileItems = allItems.Where(i => i.slotType == ItemSlotType.Projectile).ToList();

        shopItems = new List<ShopItem>
    {
        CreateShopItem(GetRandomAndRemove(passiveItems, rng), ShopSlotType.Passive),
        CreateShopItem(GetRandomAndRemove(activeItems, rng), ShopSlotType.Active),
        CreateShopItem(GetRandomAndRemove(projectileItems, rng), ShopSlotType.Projectile)
    };

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

    public bool TryPurchaseCharges(Player player, int cost, int amount)
    {
        if (player.CurrentCharges >= player.MaxCharges)
        {
            Debug.Log("Player already has full charges.");
            return false;
        }

        if (!RewardManager.Instance.SpendRunCurrency(cost))
            return false;

        RewardManager.Instance.GivePlayerCharges(player, amount);

        return true;
    }

    private ShopItem CreateShopItem(Item item, ShopSlotType slotType)
    {
        if (item == null) return null;

        return new ShopItem
        {
            item = item,
            price = item.price,
            purchased = false,
            slotType = slotType
        };
    }

    private T GetRandom<T>(List<T> list, System.Random rng)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogError($"No items found for type {typeof(T).Name}");
            return default;
        }

        return list[rng.Next(list.Count)];
    }

    private Item GetRandomAndRemove(List<Item> pool, System.Random rng)
    {
        if (pool.Count == 0)
            return null;

        int index = rng.Next(pool.Count);

        Item item = pool[index];
        pool.RemoveAt(index);

        return item;
    }
}