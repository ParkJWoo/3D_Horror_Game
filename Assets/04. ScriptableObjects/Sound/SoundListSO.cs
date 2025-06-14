using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound/SoundListData")]
public class SoundListSO : ScriptableObject
{
    public string soundListName;
    public SoundSO[] soundItems;
}
