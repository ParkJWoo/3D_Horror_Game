using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource uiSource;

    private void Awake()
    {
        
    }
    
}
