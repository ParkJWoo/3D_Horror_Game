using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Equipment : MonoBehaviour
{
    public Player player;

    public ItemInstance[] equipItems;

    public Action OnEquipHandler;
    public Action<ItemInstance> OnUnequipHandler;

    public GameObject equipWeaponObj;

    private int selectSlotNum;
    private ItemInstance selectItem;

    public UnityAction<int, ItemInstance> OnEquipUpdate;

    public void Init(Player player)
    {
        this.player = player;
        equipItems = new ItemInstance[(int)EquipType.totalData];

    }

    public bool OnEquip(ItemInstance itemData,Transform itemPos)
    {
        if (itemData == null) return false;

        EquipItemData equipItemData = itemData.itemData as EquipItemData;
        int slotNum = (int)equipItemData.equipType;
        UnEquip(slotNum, itemPos);

        switch (equipItemData.equipType)
        {
            case EquipType.visibleEquip:
                equipWeaponObj = Instantiate(equipItemData.equipModelPrefab, player.equipPos);
                if (equipWeaponObj.TryGetComponent<EquipItemHandler>(out EquipItemHandler equipItem))
                {
                    equipItem.Init(player, itemData);
                }
                break;
        }

        equipItems[slotNum] = itemData;
        Debug.Log(itemData.itemData.itemName);
        OnEquipHandler?.Invoke();
        OnEquipUpdate?.Invoke(slotNum, itemData);
        return true;
    }

    public void UnEquip(int slotNum, Transform dropPos)
    {
        if (equipItems[slotNum] == null) return;

        OnUnequipHandler?.Invoke(equipItems[slotNum]);
        Instantiate(equipItems[slotNum].itemData.dropItemPrefab, dropPos.position,dropPos.rotation);
        equipItems[slotNum] = null;
        OnEquipUpdate?.Invoke(slotNum, null);

    }

    public void SelectItem(EquipSlot slotData)
    {
        selectSlotNum = slotData.slotNum;
        selectItem = slotData.slotItem;
    }

    public void DeselectItem()
    {
        selectSlotNum = -1;
        selectItem = null;
    }
}
