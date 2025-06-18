using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManualGuide : MonoBehaviour
{
    public TextMeshProUGUI controlTextUI;
    public float displayDuration = 4f;

    [TextArea(2, 5)]
    public List<string> controlMessage = new List<string>();

    private void Start()
    {
        if (GameManager.Instance.isNewGame)
        {
            if (controlTextUI != null && controlMessage.Count > 0)
            {
                StartCoroutine(GuideStart());
            }
        }
    }

    private IEnumerator GuideStart()
    {
        yield return new WaitForSeconds(2.5f);

        if (controlTextUI != null && controlMessage.Count > 0)
        {
            StartCoroutine(ShowControlsRoutine());
        }
    }

    private IEnumerator ShowControlsRoutine()
    {
        controlTextUI.gameObject.SetActive(true);

        foreach (string msg in controlMessage)
        {
            controlTextUI.text = msg;
            yield return new WaitForSeconds(displayDuration);
        }

        //  마지막 문구가 꺼지기 전에 한 번 더 기다렸다가 꺼지도록 세팅
        yield return new WaitForSeconds(displayDuration);
        controlTextUI.gameObject.SetActive(false);      
    }
}
