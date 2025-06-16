using UnityEngine;

[CreateAssetMenu(fileName = "New Equip", menuName = ("Item/EquipItem"))]
public class EquipItemData : ItemData
{
    public override ItemType itemType => ItemType.equip;
    public EquipType equipType;
    
    public float stamina;
    public float staminaRegen;
    public float moveSpeed;

    public GameObject equipModelPrefab;
}

public enum EquipType
{
    visibleEquip,
    passvieEquip,
    totalData
}