using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SequenceText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sequenceText;

    public void SetText(string text)
    {
        sequenceText.text = text;
    }
}
