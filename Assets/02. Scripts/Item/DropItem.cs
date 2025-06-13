using System;
using System.Net.NetworkInformation;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    public ItemData item;
    public int quantity;
    public float durability;

    public event Action<IInteractable> OnInteracted;

    public void Init(ItemInstance item)
    {
        this.item = item.itemData;
        this.quantity = item.quantity;
        this.durability = item.durability;
    }

    public void OnInteraction()
    {
        ItemInstance returnItem = CharacterManager.Instance.Player.Inventory.GetItem(this);

        if(returnItem == null)
        {
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

    }
}