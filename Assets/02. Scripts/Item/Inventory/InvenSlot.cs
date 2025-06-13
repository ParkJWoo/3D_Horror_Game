using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour, IPointerClickHandler
{
    public int slotNum;
    public ItemInstance slotItem;

    public Image itemImage;
    public TextMeshProUGUI quantityText;

    public Button slotButton;

    private bool isSelect;

    public UnityAction<InvenSlot> OnSelectSlotHandler;
    public UnityAction<InvenSlot> OnDeselectSlotHandler;

    public UnityAction<InvenSlot> OnUseSlotHandler;

    public Sprite emptySprite;

    public UnityAction<InvenSlot> OnDropHandler;


    public void Init(int slotNum)
    {
        this.slotNum = slotNum;
        slotButton.onClick.AddListener(Toggle);
        itemImage.sprite = emptySprite;
        quantityText.enabled = false;
        slotButton.interactable = false;
        DeselectSlot();
    }

    public void AddItem(ItemInstance addItem)
    {
        slotItem = addItem;
        itemImage.sprite = addItem.itemData.itemImage;
    }

    public void RemoveItem()
    {
        slotItem = null;
        itemImage.sprite = emptySprite;
        quantityText.enabled = false;
        slotButton.interactable = false;
        DeselectSlot();
    }

    public void UpdateSlot(ItemInstance item)
    {
        if (item != null && item.itemData != null && item.quantity > 0)
        {
            slotItem = item;
            slotButton.interactable = true;
            itemImage.sprite = item.itemData.itemImage;

            if (slotItem.quantity > 1)
            {
                quantityText.enabled = true;
                quantityText.text = slotItem.quantity.ToString();
            }
            else
            {
                quantityText.enabled = false;
            }
        }
        else
        {
            RemoveItem();
        }
    }

    public void Toggle()
    {
        if (isSelect)
        {
            DeselectSlot();
        }
        else
        {
            SelectSlot();
        }
    }

    public void SelectSlot()
    {
        if (slotItem?.itemData == null) return;
        isSelect = true;
        OnSelectSlotHandler?.Invoke(this);
    }

    public void DeselectSlot()
    {
        isSelect = false;
        OnDeselectSlotHandler?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnUseSlotHandler?.Invoke(this);
        }
    }
}
