using UnityEngine;

[CreateAssetMenu(fileName = "New Equip", menuName = ("Item/EquipItem"))]
public class EquipItemData : ItemData
{
    public override ItemType itemType => ItemType.equip;
    public EquipType equipType;
    
    public int defence;
    public int health;
    public int stamina;
    public int moveSpeed;

    public GameObject equipModelPrefab;
}

public enum EquipType
{
    visibleEquip,
    totalData
}