using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound/SoundData")]
public class SoundSO : ScriptableObject
{
    [field: SerializeField]public string SoundName { get; private set; }
    [field: SerializeField]public AudioClip Clip { get; private set; }
    [field: SerializeField]public AudioClip[] Clips { get; private set; }
}
