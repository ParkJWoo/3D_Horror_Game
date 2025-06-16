using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperPuzzlePhoto : MonoBehaviour, IInteractable
{
    public bool isCorrectPhoto = false;             //  이 사진이 정답인지 아닌지 
    public WhisperTrigger linkedWhisperTrigger;

    public event Action<IInteractable> OnInteracted;

    public void OnInteraction()
    {
        Debug.Log("사진과 상호작용함");
        

        if (isCorrectPhoto && linkedWhisperTrigger != null)
        {
            Debug.Log("정답 사진입니다. 속삭임을 끕니다.");
            linkedWhisperTrigger.SuppressWhisperSound();
        }

        else
        {
            Debug.Log("틀린 사진입니다.");
        }

        OnInteracted?.Invoke(this);
    }

    public void SetInterface(bool active)
    {
    }
}
