using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    public Player player;
    public ItemManager itemManager;

    public int inventoryMaxSize;
    public ItemInstance[] invenItems;

    public int selectSlotNum;
    public ItemInstance selectItem;

    public UnityAction<int, ItemInstance> OnInventoryUpdate;

    public Inventory(Player player)
    {
        this.player = player;
        itemManager = PlaySceneManager.instance.itemManager;
        inventoryMaxSize = 5;
        invenItems = new ItemInstance[inventoryMaxSize];

        for (int i = 0; i < invenItems.Length; i++)
        {
            invenItems[i] = new ItemInstance(null, 0, 0);
        }

        List<SaveItemData> save = SaveManager.Instance.saveData.haveItemData;
        
        for (int i = 0; i < save.Count; i++)
        {
            invenItems[i] = new ItemInstance(itemManager.itemDataBase[save[i].itemCode], save[i].quantity, save[i].durability);
        }
        


    }

    public ItemInstance GetItem(DropItem dropItem)
    {
        ItemInstance getItem = new ItemInstance(dropItem.item, dropItem.quantity, dropItem.durability);

        if (dropItem.item.itemType == ItemType.equip)
        {
            player.Equipment.OnEquip(getItem, dropItem.transform);
            return null;
        }
        else
        {
            return AddItem(getItem);
        }
    }

    public ItemInstance AddItem(ItemInstance newItem)
    {
        if (newItem.itemData.canStack)
        {
            if (TryStackToSameItem(newItem) != null)
            {
                newItem = TryStackToEmptySlot(newItem);
            }
        }
        else
        {
            for (int i = 0; i < invenItems.Length; i++)
            {
                if (invenItems[i].itemData == null)
                {
                    invenItems[i] = newItem;
                    Debug.Log(newItem.itemData.itemName);
                    OnInventoryUpdate?.Invoke(i, newItem);
                    return null;
                }
            }
        }

        return newItem;
    }

    public ItemInstance TryStackToSameItem(ItemInstance newItem)
    {
        var sameItems = invenItems.Where(i => i != null && i.itemData == newItem.itemData);

        foreach (var invenItem in sameItems)
        {
            if (invenItem.quantity == invenItem.itemData.maxQuantity) continue;

            int stackAbleQuntity = invenItem.itemData.maxQuantity - invenItem.quantity;
            int slotNum = Array.IndexOf(invenItems, invenItem);

            int stackAmount = newItem.quantity > stackAbleQuntity ? stackAbleQuntity : newItem.quantity;

            invenItem.ChangeQuantity(stackAmount);
            newItem.ChangeQuantity(-stackAmount);

            OnInventoryUpdate?.Invoke(slotNum, invenItem);
            if (newItem.quantity == 0) return null;
        }

        return newItem;
    }

    public ItemInstance TryStackToEmptySlot(ItemInstance newItem)
    {
        for (int i = 0; i < invenItems.Length; i++)
        {
            if (invenItems[i].itemData == null)
            {
                invenItems[i] = new ItemInstance(newItem.itemData, 0, 0);
                int stackAmount = newItem.quantity > newItem.itemData.maxQuantity
                    ? newItem.itemData.maxQuantity
                    : newItem.quantity;

                invenItems[i].ChangeQuantity(stackAmount);
                newItem.ChangeQuantity(-stackAmount);

                OnInventoryUpdate?.Invoke(i, invenItems[i]);
                if (newItem.quantity == 0) return null;
            }
        }

        return newItem;
    }

    public void UseItem(int slotNum)
    {
        if (invenItems[slotNum].itemData == null) return;

        switch (invenItems[slotNum].itemData.itemType)
        {
            case ItemType.consumable:
                ConsumItemData consumableItem = invenItems[slotNum].itemData as ConsumItemData;
                invenItems[slotNum].ChangeQuantity(-1);
                foreach (ItemEffect itemEffect in consumableItem.itemEffect)
                {
                    player.ApplyUseItem(itemEffect);
                }

                break;
            case ItemType.useable:
                break;
            case ItemType.battery:
                BatteryItemData batteryItemData = invenItems[slotNum].itemData as BatteryItemData;
                if (player.Equipment.equipItemHandler?.RecoverDurability(batteryItemData.durabilityData) == true)
                {
                    invenItems[slotNum].ChangeQuantity(-1);
                }

                break;
            case ItemType.key:
                invenItems[slotNum].ChangeQuantity(-1);
                break;
        }

        if (invenItems[slotNum].quantity == 0)
        {
            RemoveItem(slotNum);
        }

        OnInventoryUpdate?.Invoke(slotNum, invenItems[slotNum]);
    }

    public void RemoveItem(int SlotNum)
    {
        invenItems[SlotNum].itemData = null;
        invenItems[SlotNum].quantity = 0;
        invenItems[SlotNum].durability = 0;
    }

    public void SelectItem(InvenSlot slotData)
    {
        selectSlotNum = slotData.slotNum;
        selectItem = slotData.slotItem;
    }

    public void DeselectItem()
    {
        selectSlotNum = -1;
        selectItem = null;
    }

    public void OnDrop(int slotNum)
    {
        itemManager.DropItem(invenItems[slotNum], player.transform.position);
        RemoveItem(slotNum);
        OnInventoryUpdate?.Invoke(slotNum, invenItems[slotNum]);
    }

    public (int, ItemInstance) FindItem(ItemData itemData)
    {
        ItemInstance findItem = invenItems.FirstOrDefault(i => i?.itemData == itemData);
        if (findItem != null)
        {
            if (findItem.itemData == itemData)
            {
                int slotNum = Array.IndexOf(invenItems, findItem);
                return (slotNum, findItem);
            }
        }

        return (-1,null);
    }
}