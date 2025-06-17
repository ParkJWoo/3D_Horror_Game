using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    [HideInInspector] public Player player;



    public ItemManager itemManager;
    [HideInInspector] public ItemInstance[] equipItems;

    public Action<EquipItemData> OnEquipHandler;
    public Action<EquipItemData> OnUnequipHandler;

    public EquipItemHandler equipItemHandler;

    private int selectSlotNum;
    private ItemInstance selectItem;

    public UnityAction<int, ItemInstance> OnEquipUpdate;

    public void Init(Player player)
    {
        this.player = player;
        equipItems = new ItemInstance[(int)EquipType.totalData];

        if (GameManager.Instance.isNewGame)
        {
            for (int i = 0; i < equipItems.Length; i++)
            {
                equipItems[i] = new ItemInstance(null, 0, 0);
            }
        }
        else
        {
            SaveItemData[] save = SaveManager.Instance.saveData.equipItemData;
            for (int i = 0; i < save.Length; i++)
            {
                equipItems[i] = new ItemInstance(itemManager.FindSOData(save[i].itemCode), save[i].quantity, save[i].durability);
                EquipItemData equipItemData = equipItems[i].itemData as EquipItemData;

                if (i == (int)EquipType.visibleEquip)
                {
                    GameObject equipModel = Instantiate(equipItemData.equipModelPrefab, player.equipPos);
                    if (equipModel.TryGetComponent<EquipItemHandler>(out EquipItemHandler equipItem))
                    {
                        equipItemHandler = equipItem;
                        equipItemHandler.Init(player, equipItems[i]);
                    }
                }
                OnEquipHandler?.Invoke(equipItemData);
                OnEquipUpdate?.Invoke(i, equipItems[i]);
            }
        }

        player.PlayerInput.playerInput.Player.Flash.started += UseItem;
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        if(equipItemHandler == null)
        {
            Debug.Log("장착한 아이템이 없습니다.");
            return;
        }

        equipItemHandler.UseItem();
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
                GameObject equipModel = Instantiate(equipItemData.equipModelPrefab, player.equipPos);
                if (equipModel.TryGetComponent<EquipItemHandler>(out EquipItemHandler equipItem))
                {
                    equipItemHandler = equipItem;
                    equipItemHandler.Init(player, itemData);
                }
                break;
        }

        equipItems[slotNum] = itemData;
        //Debug.Log(itemData.itemData.itemName);
        OnEquipHandler?.Invoke(equipItemData);
        OnEquipUpdate?.Invoke(slotNum, itemData);
        return true;
    }

    public void UnEquip(int slotNum, Transform dropPos)
    {
        if (equipItems[slotNum].itemData == null) return;

        EquipItemData equipItemData = equipItems[slotNum].itemData as EquipItemData;

        switch (equipItemData.equipType)
        {
            case EquipType.visibleEquip:
                equipItemHandler = null;
                Destroy(player.equipPos.GetChild(0));
                break;
        }

        OnUnequipHandler?.Invoke(equipItemData);
        Instantiate(equipItems[slotNum].itemData.dropItemPrefab, dropPos.position,dropPos.rotation);
        equipItems[slotNum].itemData = null;
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

    private void OnDestroy()
    {
        player.PlayerInput.playerInput.Player.Flash.started -= UseItem;
    }
}
