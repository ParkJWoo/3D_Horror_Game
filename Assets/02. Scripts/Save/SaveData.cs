using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Checkpoint
{
    None,
    FoundFlashlight,
    GetLetter,
    GetKey,
    OpenLockedDoor,
    
    
}


[Serializable]
public class SaveData
{
    //저쟝할 데이터 추가 예정
    public Vector3 playerPosition;
    public float flashlightBattery;
    public List<ItemData> items;
    public Checkpoint lastCheckpoint;
    public float currentBgmVolume;
    public float currentSfxVolume;
    public bool currentBgmMute;
    public bool currentSfxMute;
}
