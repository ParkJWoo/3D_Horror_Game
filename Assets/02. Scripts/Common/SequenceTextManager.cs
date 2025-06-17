using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SequenceTextManager : MonoBehaviour
{
    public Transform sequencetextPos;
    public SequenceText sequencetextPrefab;
    public float textLifeTime = 5f;

    public void SetSequenceText(string sequenceText)
    {
        if (sequencetextPos.childCount >= 3)
        {
            Destroy(sequencetextPos.GetChild(0).gameObject);
        }

        var sequnceText = Instantiate(sequencetextPrefab, sequencetextPos);
        sequnceText.SetText(sequenceText);
        Destroy(sequnceText.gameObject, textLifeTime);
    }
}
