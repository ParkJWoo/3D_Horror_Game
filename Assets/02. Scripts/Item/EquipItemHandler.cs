using UnityEngine;

public abstract class EquipItemHandler : MonoBehaviour
{
    public Player player;
    public ItemInstance item;

    public void Init(Player player, ItemInstance item)
    {
        this.player = player;
        this.item = item;
    }

    public abstract void UseItem();
}
