using UnityEngine;

public class Shop : MonoBehaviour
{
    public void BuyItem(UnitBase unit, Item item)
    {
        // Example: check if player has enough gold here
        // if (player.gold >= item.price) { ... }

        unit.AddItemToRunData(item);

        item.OnAcquire(unit);

        Debug.Log($"{unit.name} bought {item.itemName}");
    }
}
