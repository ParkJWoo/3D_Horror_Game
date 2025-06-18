using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    public ItemData item;
    public int quantity;
    public float durability;

    public event Action<IInteractable> OnInteracted;
    public event Action<DropItem> OnDestoryItem;

    public void Init(ItemInstance item)
    {
        this.item = item.itemData;
        this.quantity = item.quantity;
        this.durability = item.durability;
    }

    public void OnInteraction()
    {
        ItemInstance returnItem = CharacterManager.Instance.Player.Inventory.GetItem(this);

        if (returnItem == null)
        {
            OnDestoryItem?.Invoke(this);
            Destroy(gameObject);
        }
        else
        {
            quantity = returnItem.quantity;
        }
        OnInteracted?.Invoke(this);
    }

    public void SetInterface(bool active)
    {
        string itemDescription = "";

        if (active)
        {
            itemDescription = $"[E] {item.itemName} \n {item.itemDescription}";
        }

        UIManager.Instance.SetInteractionText(active, itemDescription);
    }
}