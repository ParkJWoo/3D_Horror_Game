using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperTrigger : MonoBehaviour
{
    public string soundName = "Whisper";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WhisperTrigger 진입 → 사운드 재생");
            SoundManager.Instance.PlayLoopSfx(soundName);       //  LoopSfxSource로 재생한다.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("WhisperTrigger 이탈 → 사운드 정지");
            SoundManager.Instance.StopLoopSfx();
        }
    }
}
