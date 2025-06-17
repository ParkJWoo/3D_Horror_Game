using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideManager : Singleton<GuideManager>
{
    [Header("UI")]
    public TextMeshProUGUI guideTextUI;
    public float displayDuration = 4f;

    [Header("Guide Content")]
    [TextArea(2, 4)]
    public List<string> guideTexts = new List<string>();

    private int currentIndex = 0;
    private Coroutine currentCoroutine;
    private bool isDisplaying = false;

    private void Start()
    {
        if (guideTextUI != null)
        {
            guideTextUI.gameObject.SetActive(false);
        }
    }

    //  처음부터 가이드 시퀀스 자동 실행
    public void StartGuideSequence()
    {
        if (guideTexts.Count == 0 || isDisplaying)
        {
            return;
        }

        currentIndex = 0;
        ShowGuideAt(currentIndex);
    }

    //  외부에서 수동으로 다음 가이드 호출
    public void ShowNextGuide()
    {
        if (isDisplaying || currentIndex >= guideTexts.Count - 1)
        {
            return;
        }

        currentIndex++;
        ShowGuideAt(currentIndex);
    }

    //  특정 인덱스의 가이드를 출력
    private void ShowGuideAt(int index)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(GuideRoutine(index));
    }

    private IEnumerator GuideRoutine(int index)
    {
        isDisplaying = true;

        guideTextUI.text = guideTexts[index];
        guideTextUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        guideTextUI.gameObject.SetActive(false);
        isDisplaying = false;
    }

    public void ForceHideGuide()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        guideTextUI.text = "";
        guideTextUI.gameObject.SetActive(false);
        isDisplaying = false;
    }
}
