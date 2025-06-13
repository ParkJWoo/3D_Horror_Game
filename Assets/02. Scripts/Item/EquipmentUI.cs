using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentUI : MonoBehaviour
{
    private Player player;
    private Equipment equipment;

    public EquipSlot equipSlotPrefab;
    public Transform equipSlotPosition;
    public List<EquipSlot> equipSlots = new List<EquipSlot>();

    public void Init(Player player)
    {
        this.player = player;
        equipment = player.Equipment;
        equipment.OnEquipUpdate += OnUpdateSlot;

        for (int i = 0; i < (int)EquipType.totalData; i++)
        {
            equipSlots.Add(Instantiate(equipSlotPrefab, equipSlotPosition));
            equipSlots[i].Init(i);
            equipSlots[i].UpdateSlot(equipment.equipItems[i]);
        }
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void OnUpdateSlot(int slot, ItemInstance slotItem)
    {
        equipSlots[slot].UpdateSlot(slotItem);
    }
}