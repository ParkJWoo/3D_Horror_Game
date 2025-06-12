using System;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    
    public ItemInstance item;

    public event Action<IInteractable> OnInteracted;

    public void Init(ItemInstance item)
    {
        this.item = item;
    }

    public void OnInteraction()
    {
        //int used = 0;

        //if (player.inventory.AddItem(item, out used))
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    item.ChangeQauntity(used);
        //}

        //OnInteracted?.Invoke(this);
    }

    public void SetInterface(bool active)
    {

    }
}