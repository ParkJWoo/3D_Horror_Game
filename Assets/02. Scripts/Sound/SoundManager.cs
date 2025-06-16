using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

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
    }
    
    // Enemy에서 호출할 메서드
    public void Play3DSound(string soundName, Vector3 soundPosition, Transform listener, float maxDistance)
    {
        AudioClip clip =  GetSound(soundName);
        float distance = Vector3.Distance(soundPosition, listener.position);

        float volume = Mathf.Clamp01(1 - (distance/maxDistance));
        sfxSource.PlayOneShot(clip, volume);
    }
    
    public void Play3DSound(string soundName, Vector3 soundPosition)
    {
        AudioClip clip = GetSound(soundName);
        AudioSource.PlayClipAtPoint(clip, soundPosition);
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

        StartCoroutine(FadeOutSfx());
    }

    private IEnumerator FadeOutSfx()
    {
        float startVolume = sfxSource.volume;
        float fadeTime = 0.7f;

        while (sfxSource.volume > 0)
        {
            sfxSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        sfxSource.Stop();
        sfxSource.volume = startVolume;
    }
    
    // BGM을 바꿔주는 메서드
    public void SwitchBgm(string soundName)
    {
        float fadeTime = 0.3f;
        StartCoroutine(FadeOutBgm(soundName, fadeTime));
    }
    
    public void SwitchBgm(string soundName, float fadeTime)
    {
        StartCoroutine(FadeOutBgm(soundName, fadeTime));
    }

    private IEnumerator FadeOutBgm(string soundName, float fadeTime)
    {
        float startVolume = bgmSource.volume;

        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        StopBgmLoop();
        PlayBgmLoop(soundName);

        while (bgmSource.volume < startVolume)
        {
            bgmSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        bgmSource.volume = startVolume;

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
    }

    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
        Debug.Log($"브금볼륨인풋값{volume}");
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
        Debug.Log($"효과음볼륨인풋값{volume}");
    }

    public bool IsBgmMute()
    {
        return bgmSource.mute;
    }

    public bool IsSfxMute()
    {
        return sfxSource.mute;
    }
}