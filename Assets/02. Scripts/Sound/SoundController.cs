using UnityEngine;

public class SoundController : MonoBehaviour
{ 
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SoundPlay(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    public void PlayLoop(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.clip = sound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    
    public void StopLoop()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }
}
