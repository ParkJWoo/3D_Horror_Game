using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource enemyAudioSource;

    [SerializeField] private List<SoundSO> sound;
    private Dictionary<string, AudioClip> soundDictionary;
    private Dictionary<string, AudioClip[]> soundsDictionary;


    public void Awake()
    {
        soundDictionary = sound.ToDictionary(sound => sound.SoundName, sound => sound.Clip);
        soundsDictionary = sound.ToDictionary(sound => sound.SoundName, sound => sound.Clips);
    }

    // 사운드 효과 호출 메서드
    public void PlaySound(string soundName)
    {
        AudioClip clip = GetSound(soundName);
        sfxSource.PlayOneShot(clip);
    }
    
    public void PlayBgmLoop(string soundName)
    {
        AudioClip clip = GetSound(soundName);
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        
    }

    public void StopBgmLoop()
    {
        bgmSource.loop = false;
        bgmSource.Stop();
    }

    // 씬 전환시 호출할 메서드
    public void UnloadAllSounds()
    {
        bgmSource.Stop();
        sfxSource.Stop();
        enemyAudioSource.Stop();
    }
    
    // Enemy에서 호출할 메서드

    public void PlayEnemySound(string soundName)
    {
        if (!enemyAudioSource.gameObject.activeInHierarchy) return;
        AudioClip clip = GetSound(soundName);
        enemyAudioSource.PlayOneShot(clip);
    }

    public void PlayLoopEnemySound(string soundName)
    {
        if (!enemyAudioSource.gameObject.activeInHierarchy) return;
        AudioClip clip = GetSound(soundName);
        enemyAudioSource.clip = clip;
        enemyAudioSource.loop = true;
        enemyAudioSource.Play();
    }

    public void StopLoopEnemySound()
    {
        if (!enemyAudioSource.gameObject.activeInHierarchy) return;
        enemyAudioSource.loop = false;
        StartCoroutine(FadeOut(enemyAudioSource, 0.1f));
    }
    

    public void PlayLoopSfx(string soundName)
    {
        AudioClip clip = GetSound(soundName);
        sfxSource.loop = true;
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void StopLoopSfx()
    {
        sfxSource.loop = false;

        StartCoroutine(FadeOut(sfxSource, 0.7f));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    
    // BGM을 바꿔주는 메서드
    public void SwitchBgm(string soundName)
    {
        float fadeTime = 0.3f;
        StartCoroutine(SmoothChangeAudio(bgmSource, soundName, fadeTime));
    }
    
    public void SwitchBgm(string soundName, float fadeTime)
    {
        StartCoroutine(SmoothChangeAudio(bgmSource, soundName, fadeTime));
    }

    private IEnumerator SmoothChangeAudio(AudioSource audioSource, string soundName, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        StopBgmLoop();
        PlayBgmLoop(soundName);

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        audioSource.volume = startVolume;

    }

    public void PlayRandomSound(string soundName)
    {
        AudioClip[] clips = GetSounds(soundName);
        sfxSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
    
    private AudioClip GetSound(string soundName)
    {
        soundDictionary.TryGetValue(soundName, out AudioClip clip);
        return clip;
    }

    private AudioClip[] GetSounds(string soundName)
    {
        soundsDictionary.TryGetValue(soundName, out AudioClip[] clips);
        return clips;
    }
    

    public void ToggleBgmMute()
    {
        bgmSource.mute = !bgmSource.mute;
    }

    public void ToggleSfxMute()
    {
        sfxSource.mute = !sfxSource.mute;
        enemyAudioSource.mute = !enemyAudioSource.mute;
    }

    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
        Debug.Log($"브금볼륨인풋값{volume}");
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
        enemyAudioSource.volume = volume;
        Debug.Log($"효과음볼륨인풋값{volume}");
    }

    public bool IsBgmMute()
    {
        return bgmSource.mute;
    }

    public bool IsSfxMute()
    {
        return sfxSource.mute && enemyAudioSource.mute;
    }
}