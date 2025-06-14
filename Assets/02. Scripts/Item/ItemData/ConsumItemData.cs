using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consum", menuName = ("Item/ConsumableItem"))]
public class ConsumItemData : ItemData
{
    public override ItemType itemType => ItemType.consumable;
    public List<ItemEffect> itemEffect;
}

[Serializable]
public class ItemEffect
{
    public ItemEffectType itemEffectType;
    public int amount;
    public int duration;
}

public enum ItemEffectType
{
    stamina,
    moveSpeed,
    staminaRegen
}

