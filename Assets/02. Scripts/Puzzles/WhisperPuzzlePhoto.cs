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
    public int photoNum;
    private string[] senarioText;

    public event Action<IInteractable> OnInteracted;

    private void Start()
    {
        switch (photoNum)
        {
            case 0:
                senarioText = Constants.stageTwoText;
                break;
            case 1:
                senarioText = Constants.stageThreeText;
                break;
            case 2:
                senarioText = Constants.stageFourText;
                break;
        }
    }

    public void OnInteraction()
    {
        if (isCorrectPhoto && linkedWhisperTrigger != null)
        {
            linkedWhisperTrigger.SuppressWhisper();
        }

        if(iskeyphoto)
        {
            (UIManager.Instance as PlayScnenUIManager).sequenceTextManager.SetScenarioText(senarioText);
            mapcontroller.UpdateLastCheckpoint(photoNum);
            mapcontroller.GoNextStage();
            iskeyphoto = false;
        }

        OnInteracted?.Invoke(this);
    }

    public void SetInterface(bool active)
    {
    }
}
