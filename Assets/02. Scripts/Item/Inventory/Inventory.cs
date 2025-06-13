using System;
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
    }

    public ItemInstance AddItem(ItemInstance newItem)
    {
        if (newItem.itemData.canStack)
        {
            var sameItems = invenItems.Where(i => i != null && i.itemData == newItem.itemData);

            foreach (var invenItem in sameItems)
            {
                if (invenItem.quantity == invenItem.itemData.maxQuantity)
                {
                    continue;
                }
                else
                {
                    int stackAbleQuntity = invenItem.itemData.maxQuantity - invenItem.quantity;
                    int slotNum = Array.IndexOf(invenItems, invenItem);

                    if (newItem.quantity > stackAbleQuntity)
                    {
                        invenItem.ChangeQuantity(stackAbleQuntity);
                        newItem.ChangeQuantity(-stackAbleQuntity);
                        OnInventoryUpdate?.Invoke(slotNum, invenItem);
                    }
                    else
                    {
                        invenItem.ChangeQuantity(newItem.quantity);
                        OnInventoryUpdate?.Invoke(slotNum, invenItem);
                        return null;
                    }
                }
            }
        }

        for (int i = 0; i < invenItems.Length; i++)
        {
            if (invenItems[i] == null)
            {
                invenItems[i] = newItem;
                Debug.Log(newItem.itemData.itemName);
                OnInventoryUpdate?.Invoke(i, newItem);
                return null;
            }
        }

        return newItem;
    }

    public void UseItem()
    {
        if (selectItem == null) return;

        switch (selectItem.itemData.itemType)
        {
            case ItemType.consumable:
                ConsumItemData consumableItem = selectItem.itemData as ConsumItemData;
                selectItem.ChangeQuantity(-1);
                foreach (ItemEffect itemEffect in consumableItem.itemEffect)
                {
                    //player.stat.ApplayItemEffect(itemEffect);
                }
                break;
            case ItemType.useable:
                break;
        }

        if (selectItem.quantity == 0)
        {
            RemoveItem(selectSlotNum);
        }

        OnInventoryUpdate?.Invoke(selectSlotNum, selectItem);
    }

    public void UseItem(int slotNum)
    {
        if (invenItems[slotNum] == null) return;

        switch (invenItems[slotNum].itemData.itemType)
        {
            case ItemType.consumable:
                ConsumItemData consumableItem = invenItems[slotNum].itemData as ConsumItemData;
                invenItems[slotNum].ChangeQuantity(-1);
                foreach (ItemEffect itemEffect in consumableItem.itemEffect)
                {
                    //player.stat.ApplayItemEffect(itemEffect);
                }
                break;
            case ItemType.useable:

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
        invenItems[SlotNum] = null;
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

    public ItemInstance FindItem(ItemData itemData)
    {
        ItemInstance findItem = invenItems.FirstOrDefault(i => i.itemData == itemData);
        if (findItem != null)
        {
            if (findItem.itemData == itemData)
            {
                return findItem;
            }
        }

        return null;
    }
}
