using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeathEffectManager : MonoBehaviour
{
    public CanvasGroup deathPanelGroup;         //  DeadScreenPanel에 CanvasGroup 추가
    public Image deadImage;
    //public AudioSource audioSource;
    //public AudioClip deathSFX;
    public float fadeSpeed = 2f;

    private void Awake()
    {
        deathPanelGroup.alpha = 0;
        deadImage.enabled = false;
    }

    public void PlayDeathSequence()
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        //  검은 화면 페이드 인
        float timer = 0f;
        deathPanelGroup.gameObject.SetActive(true);
        deadImage.enabled = true;

        while (timer < 1f)
        {
            timer += Time.unscaledDeltaTime;
            deathPanelGroup.alpha = Mathf.Lerp(0, 1, timer / 1f);
            yield return null;
        }

        ////  사망 효과음 재생
        //if(audioSource && deathSFX)
        //{
        //    audioSource.PlayOneShot(deathSFX);
        //}

        //  게임 정지
        Time.timeScale = 0f;
    }
}
