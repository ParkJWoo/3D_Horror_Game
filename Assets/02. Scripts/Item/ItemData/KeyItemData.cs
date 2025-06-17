using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equip", menuName = ("Item/KeyItem"))]
public class KeyItemData : ItemData
{
    public override ItemType itemType => ItemType.key;
    public int keyType;
}