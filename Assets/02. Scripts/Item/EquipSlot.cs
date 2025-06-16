using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public int slotNum;
    public ItemInstance slotItem;

    public Image itemImage;
   
    public Sprite emptySprite;

    public void Init(int slotNum)
    {
        this.slotNum = slotNum;
        itemImage.sprite = emptySprite;
    }

    public void AddItem(ItemInstance addItem)
    {
        slotItem = addItem;
        itemImage.sprite = addItem.itemData.itemImage;
    }


    public void UpdateSlot(ItemInstance item)
    {
        if (item != null && item.itemData != null)
        {
            slotItem = item;
            //Debug.Log(item.itemData);
            itemImage.sprite = item.itemData.itemImage;
        }
        else
        {
            UnEquip();
        }
    }

    public void UnEquip()
    {
        slotItem = null;
        itemImage.sprite = emptySprite;
    }
 }
