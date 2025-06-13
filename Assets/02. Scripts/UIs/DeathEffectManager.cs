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
        //  패널은 항상 Active 상태로
        //  시작 시, 완전히 투명, 이미지는 표시할 것
        deathPanelGroup.alpha = 0;

        if(deadImage != null)
        {
            deadImage.enabled = true;
        }
    }

    public void PlayDeathSequence()
    {
        //  활성화 상태일 때만 코루틴 실행할 것!
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        deathPanelGroup.alpha = 0f;

        float timer = 0f;

        while (timer < 2f)
        {
            timer += Time.unscaledDeltaTime * fadeSpeed;
            float time = Mathf.Clamp01(timer / 1f);

            //  더 부드러운 곡선형태로 띄움 (SineInOut)
            float eased = 0.5f - 0.5f * Mathf.Cos(Mathf.PI * time);
            deathPanelGroup.alpha = eased;

            //  옵션 (안 넣어도 됨) 붉은 빛 강조
            if(deadImage != null)
            {
                deadImage.color = Color.Lerp(Color.clear, new Color(0.8f, 0, 0, 1), eased);
            }

            yield return null;
        }

        deathPanelGroup.alpha = 1f;

        if(deadImage != null)
        {
            deadImage.color = new Color(0.8f, 0, 0, 1);
        }

        Time.timeScale = 0f;
    }
}
