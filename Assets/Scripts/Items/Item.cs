using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public string description;

    public int price = 100;

    public ItemSlotType slotType;

    public abstract void OnAcquire(UnitBase unit);
}