using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperTrigger : MonoBehaviour
{
    public AudioClip[] whisperClips;
    public float whisperInterval = 6f;

    private AudioSource audioSource;
    private float timer;
    private bool isPlayerInside = false;

    private int loopCount = 0;              //  루프 반복 수

    private void Start()
    {
        //  AudioSource 동적 생성
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
        audioSource.volume = 1f;
        audioSource.playOnAwake = false;

        if (whisperClips.Length > 0)
        {
            PlayWhisperLoop();        //  초기 한 번 재생
        }

        else
        {
            Debug.LogWarning("No whisper clips assigned!");
        }
    }

    private void Update()
    {
        if(isPlayerInside)
        {
            timer += Time.deltaTime;

            if(timer >= whisperInterval && !audioSource.isPlaying)
            {
                loopCount++;
                PlayWhisperLoop();
                timer = 0f;
            }
        }
    }

    private void PlayWhisperLoop()
    {
        if (whisperClips.Length == 0)
        {
            return;
        }

        //  루프 횟수에 따라 점점 더 무서운 클립 선택
        int clipIndex = Mathf.Clamp(loopCount, 0, whisperClips.Length - 1);
        AudioClip clip = whisperClips[clipIndex];
        audioSource.clip = clip;

        //  루프 횟수에 따라 점점 더 빠른 재생 속도
        audioSource.pitch = 1f + (loopCount * 0.05f);

        audioSource.Play();

        Debug.Log($"[Whisper] Clip: {clip.name}, Pitch: {audioSource.pitch}, Loop Count: {loopCount}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player entered whisper zone.");
            isPlayerInside = true;
            loopCount++;
            PlayWhisperLoop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInside = false;
            audioSource.Stop();
        }
    }
}
