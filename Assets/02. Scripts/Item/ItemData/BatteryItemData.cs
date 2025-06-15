using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consum", menuName = ("Item/BatteryItem"))]
public class BatteryItemData : ItemData
{
    public override ItemType itemType => ItemType.battery;
    public DurabilityData durabilityData;
}

[Serializable]
public class DurabilityData
{
    public DurabilityType durabiliyType;
    public int amount;
}

public enum DurabilityType
{
    flashlight,
    firelighter
}
