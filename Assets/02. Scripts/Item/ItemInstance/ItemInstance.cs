using System;
using UnityEngine;

[Serializable]
public class ItemInstance
{
    public ItemData itemData;
    public int quantity;
    public float durability;

    public ItemInstance(ItemData itemData, int quantity, float durablility)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        this.durability = durablility;
    }

    public void ChangeQuantity(int quantity)
    {
        this.quantity += quantity;
        if (this.quantity < 0) Debug.LogError("개수가 음수입니다.");
    }

    public void ChangeDurability(float durability)
    {
        this.durability += durability;

        if (this.durability < 0f)
        {
            this.durability = 0;
            return;
        }

        if(this.durability > itemData.maxDurability)
        {
            this.durability = itemData.maxDurability;
            return;
        }
    }

    public bool GetDurability()
    {
        return durability > 0;
    }
}
