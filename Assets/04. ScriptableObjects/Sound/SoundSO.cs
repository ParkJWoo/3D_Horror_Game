using UnityEngine;

[CreateAssetMenu(fileName = "Sound/SoundData")]
public class SoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip clip;

    public AudioClip[] clips;
}
