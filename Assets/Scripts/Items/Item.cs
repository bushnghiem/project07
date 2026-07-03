using UnityEngine;
using System.Text;

public abstract class Item : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public string description;

    public int price = 100;

    public ItemSlotType slotType;

    public virtual string GetTooltipText(int shopPrice)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(description);
        sb.AppendLine();
        sb.AppendLine($"Type: {slotType}");
        sb.AppendLine($"Price: {shopPrice}");

        return sb.ToString();
    }

    public abstract void OnAcquire(UnitBase unit);
}