using UnityEngine;

public enum ShopSlotType
{
    Passive,
    Active,
    Projectile
}

[System.Serializable]
public class ShopItem
{
    public Item item;
    public int price;
    public bool purchased;

    public ShopSlotType slotType;
}
