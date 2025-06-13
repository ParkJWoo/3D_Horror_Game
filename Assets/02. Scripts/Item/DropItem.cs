using System;
using System.Net.NetworkInformation;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    private Player player => CharacterManager.Instance.Player;
    public ItemInstance item;

    public event Action<IInteractable> OnInteracted;

    public void Init(ItemInstance item)
    {
        this.item = item;
    }

    public void OnInteraction()
    {
        switch (item.itemData.itemType)
        {
            case ItemType.equip:
                EquipItemInteraction();
                break;
            
            default:
                OtherItemInteraction();
                break;
        }
        
        OnInteracted?.Invoke(this);
    }

    private void EquipItemInteraction()
    {
        player.Equipment.OnEquip(item, transform);
    }

    private void OtherItemInteraction()
    {
        item = player.Inventory.AddItem(item);
        if (item == null)
        {
            Destroy(gameObject);
        }
    }

    public void SetInterface(bool active)
    {

    }
}