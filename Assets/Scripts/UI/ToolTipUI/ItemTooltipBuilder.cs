using UnityEngine;

public static class ItemTooltipBuilder
{
    public static TooltipData Build(Item item)
    {
        return new TooltipData(
            item.itemName,
            item.GetTooltipText());
    }

    public static TooltipData Build(ShopItem item)
    {
        return new TooltipData(
            item.item.itemName,
            item.item.GetTooltipText(item.price));
    }
}
