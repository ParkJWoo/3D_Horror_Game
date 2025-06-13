using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image faderImage;        //페이더 이미지
    public float fadeInTime = 1f;   //페이드 인 완료까지 필요한 시간
    public float fadeOutTime = 2f;  //페이드 아웃 완료까지 필요한 시간

    private float fadeProgressTime; //페이드 동작 진행 상황

    private Color currentAlpha;     //페이드 연출에 필요한 알파 컬러

    private float fadeAlpha;        //페이드 진행에 따른 알파 값
    private float fadeStartTime;    //페이드 시작 시간

    public void FadeIn(Action onComplete = null)                            //액션 함수가 없으면 입력하지 않아도 작동하도록 처리
    {
        //Debug.Log("Fade In");
        StartCoroutine(FadeInHandler(onComplete));
    }

    private IEnumerator FadeInHandler(Action onComplete)
    {
        fadeProgressTime = 0.0f;                                            //페이드 인을 진행하면 알파값을 0->1로 조절하여야 하기 때문에 0으로 초기화  
        currentAlpha = faderImage.color;                                    //알파만 조절하기 위한 r g b 값을 저장

        yield return null;

        fadeStartTime = Time.realtimeSinceStartup;                          //페이드가 시작된 시간을 저장

        while (fadeProgressTime < fadeInTime)
        {
            fadeProgressTime = Time.realtimeSinceStartup - fadeStartTime;   //현재 시간과 페이드가 시작된 시간차이를 계산하여 경과 시간 계산

            fadeAlpha = Mathf.Clamp01(fadeProgressTime / fadeInTime);       //경과시간/필요시간 을 통해 0~1사이의 알파값을 구한다.

            currentAlpha.a = fadeAlpha;
            faderImage.color = currentAlpha;

            yield return null;
        }

        currentAlpha.a = 1;                                                  //while문 이후 확실하게 알파값을 처리해 주고 마무리한다.
        faderImage.color = currentAlpha;

        yield return null;

        onComplete?.Invoke();
    }

    public void FadeOut(Action onComplete = null)
    {
        //Debug.Log("Fade Out");
        StartCoroutine(FadeOutHandler(onComplete));
    }

    private IEnumerator FadeOutHandler(Action onComplete)
    {
        fadeProgressTime = 0.0f;
        currentAlpha = faderImage.color;

        yield return null;

        fadeStartTime = Time.realtimeSinceStartup;

        while (fadeProgressTime < fadeOutTime)
        {
            fadeProgressTime = Time.realtimeSinceStartup - fadeStartTime;

            fadeAlpha = 1f - Mathf.Clamp01(fadeProgressTime / fadeOutTime); //fade out 은 알파가 0으로 되어야 하기 때문에 1을 빼준다.

            currentAlpha.a = fadeAlpha;
            faderImage.color = currentAlpha;

            yield return null;
        }

        currentAlpha.a = 0;
        faderImage.color = currentAlpha;

        yield return null;

        onComplete?.Invoke();
    }
}
