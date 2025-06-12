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
        //itemManager = GameManager.Instance.itemManager;
        inventoryMaxSize = 16;
        invenItems = new ItemInstance[inventoryMaxSize];
    }

    public bool AddItem(ItemInstance newItem, out int usedQuantity)
    {
        usedQuantity = 0;

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
                        invenItem.ChangeQauntity(stackAbleQuntity);
                        newItem.ChangeQauntity(-stackAbleQuntity);
                        usedQuantity += stackAbleQuntity;
                        OnInventoryUpdate?.Invoke(slotNum, invenItem);
                    }
                    else
                    {
                        invenItem.ChangeQauntity(newItem.quantity);
                        OnInventoryUpdate?.Invoke(slotNum, invenItem);
                        return true;
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
                return true;
            }
        }

        return false;
    }

    public bool AddItem(ItemInstance newItem)
    {
        for (int i = 0; i < invenItems.Length; i++)
        {
            if (invenItems[i] == null)
            {
                invenItems[i] = newItem;
                Debug.Log(newItem.itemData.itemName);
                OnInventoryUpdate?.Invoke(i, newItem);
                return true;
            }
        }

        return false;
    }

    public void UseItem()
    {
        if (selectItem == null) return;

        switch (selectItem.itemData.itemType)
        {
            case ItemType.equip:

                //if (player.equipment.OnEquip(selectItem))
                //{
                //    RemoveItem(selectSlotNum);
                //}
                break;
            case ItemType.consumable:
                ConsumItemData consumableItem = selectItem.itemData as ConsumItemData;
                selectItem.ChangeQauntity(-1);
                foreach (ItemEffect itemEffect in consumableItem.itemEffect)
                {
                    //player.stat.ApplayItemEffect(itemEffect);
                }
                break;
            case ItemType.material:
                break;
        }

        if (selectItem.itemData.itemType != ItemType.equip && selectItem.quantity == 0)
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
            case ItemType.equip:

                //if (player.equipment.OnEquip(invenItems[slotNum]))
                //{
                //    RemoveItem(slotNum);
                //}
                break;
            case ItemType.consumable:
                ConsumItemData consumableItem = invenItems[slotNum].itemData as ConsumItemData;
                invenItems[slotNum].ChangeQauntity(-1);
                foreach (ItemEffect itemEffect in consumableItem.itemEffect)
                {
                    //player.stat.ApplayItemEffect(itemEffect);
                }
                break;
            case ItemType.material:
                break;
        }

        if (invenItems[slotNum] == null || invenItems[slotNum].quantity == 0)
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
