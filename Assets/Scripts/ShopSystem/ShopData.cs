using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    public Vector2Int gridPosition;
    public List<ShopItem> shopItems = new();
    public int rerollCount = 0;
}
