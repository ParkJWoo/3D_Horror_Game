using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public int itemCode;
    public string itemName;
    public string itemDescription;
    public string itemDetailDescription;

    public Sprite itemImage;

    public abstract ItemType itemType { get; }

    public bool canStack;
    public int maxQuantity = 1;

    public GameObject dropItemPrefab;
}

public enum ItemType
{
    equip,
    consumable,
    useable
}
