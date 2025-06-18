using UnityEngine;

public abstract class EquipItemHandler : MonoBehaviour
{
    public Player player;
    public ItemInstance item;

    public virtual void Init(Player player, ItemInstance item)
    {
        this.player = player;
        this.item = item;
    }

    public abstract void UseItem();

    public abstract bool RecoverDurability(DurabilityData durabilityData);
}
