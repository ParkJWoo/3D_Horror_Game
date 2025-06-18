using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperPuzzlePhoto : MonoBehaviour, IInteractable
{
    public bool isCorrectPhoto = false;             //  이 사진이 정답인지 아닌지 
    public WhisperEffectController linkedWhisperTrigger;
    public MapController mapcontroller;
    public bool iskeyphoto = false;

    public event Action<IInteractable> OnInteracted;

    public void OnInteraction()
    {
        if (isCorrectPhoto && linkedWhisperTrigger != null)
        {
            linkedWhisperTrigger.SuppressWhisper();
        }

        if(iskeyphoto)
        {
            mapcontroller.GoNextStage();
            iskeyphoto = false;
        }

        OnInteracted?.Invoke(this);
    }

    public void SetInterface(bool active)
    {
    }
}
