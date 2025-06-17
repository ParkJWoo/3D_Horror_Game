using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveItemData
{
    public int itemCode;
    public int quantity;
    public float durability;

    public SaveItemData(ItemInstance item)
    {
        itemCode = item.itemData == null ? -1 : item.itemData.itemCode;
        quantity = item.quantity;
        durability = item.durability;
    }
}

[Serializable]
public class SaveData
{
    //저쟝할 데이터 추가 예정
    public Vector3 playerPosition;
    public SaveItemData[] equipItemData = new SaveItemData[2];
    public List<SaveItemData> haveItemData;
    public int lastCheckpoint;
}

[Serializable]
public class OptionData
{
    public float currentBgmVolume;
    public float currentSfxVolume;
    public bool currentBgmMute;
    public bool currentSfxMute;
    
}
