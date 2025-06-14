using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    private Player player;

    private Inventory inventory;
    private InventoryDetailView inventoryDetailView;
   
    public Transform invenSlotPosition;
    public InvenSlot invenSlotPrefab;

    public List<InvenSlot> invenSlots = new List<InvenSlot>();

    private bool isOpen;

    public InvenSlot selectSlot;

    public void Init(Player player)
    {
        this.player = player;
        inventory = player.Inventory;
        player.PlayerInput.playerInput.Player.Inventory.started += Toggle;
        inventory.OnInventoryUpdate += OnUpdateSlot;
        inventoryDetailView = PlaySceneManager.instance.uiManager.inventoryDetailView;

        for (int i = 0; i < inventory.inventoryMaxSize; i++)
        {
            invenSlots.Add(Instantiate(invenSlotPrefab, invenSlotPosition));
            invenSlots[i].Init(i);
            invenSlots[i].UpdateSlot(inventory.invenItems[i]);
            invenSlots[i].OnSelectSlotHandler += OnSelectSlot;
            invenSlots[i].OnDeselectSlotHandler += inventoryDetailView.SetDetailInfo;
            invenSlots[i].OnDeselectSlotHandler += OnDeselectSlot;
            invenSlots[i].OnUseSlotHandler += OnUseSlot;
            invenSlots[i].OnDropHandler += OnDropItem;
        }
        CloseUI();
    }


    public void Toggle(InputAction.CallbackContext context)
    {
        if (isOpen)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    public void OpenUI()
    {
        isOpen = true;
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        isOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);

        selectSlot?.DeselectSlot();
        selectSlot = null;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void OnUpdateSlot(int slot, ItemInstance slotItem)
    {
        invenSlots[slot].UpdateSlot(slotItem);
    }

    public void OnSelectSlot(InvenSlot selectSlot)
    {
        if (this.selectSlot == selectSlot)
        {
            OnDeselectSlot(selectSlot);
            return;
        }

        this.selectSlot?.DeselectSlot();
        this.selectSlot = selectSlot;
        inventory.SelectItem(selectSlot);
        
    }

    public void OnDeselectSlot(InvenSlot deselectSlot)
    {
        this.selectSlot = null;
        inventory.DeselectItem();
    }

    private void OnUseSlot(InvenSlot useSlot)
    {
        inventory.UseItem(useSlot.slotNum);
    }

    private void OnDropItem(InvenSlot dropSlot)
    {
        inventory.OnDrop(dropSlot.slotNum);
    }

    private void OnDestroy()
    {
        player.PlayerInput.playerInput.Player.Inventory.started -= Toggle;
    }
}
