using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperTrigger : MonoBehaviour
{
    public string soundName = "Whisper";
    public float whisperInterval = 6f;

    private float timer;
    private bool isPlayerInside = false;
    private int loopCount = 0;

    private void Update()
    {
        if(isPlayerInside)
        {
            timer += Time.deltaTime;

            if(timer >= whisperInterval)
            {
                loopCount++;
                PlayWhisper();
                timer = 0;
            }
        }
    }

    private void PlayWhisper()
    {
        if(string.IsNullOrEmpty(soundName))
        {
            Debug.LogWarning("여기서 에러납니다!");
            return;
        }

        //  pitch 조절 없이 재생만
        SoundManager.Instance.PlaySound(soundName);
        Debug.Log($"[Whisper] Played: {soundName}, Loop Count: {loopCount}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player entered whisper zone.");
            isPlayerInside = true;
            loopCount++;
            PlayWhisper();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}
