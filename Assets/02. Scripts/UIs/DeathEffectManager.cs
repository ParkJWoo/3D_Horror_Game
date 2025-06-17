using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeathEffectManager : MonoBehaviour
{
    public CanvasGroup deathPanelGroup;         // DeadScreenPanel에 CanvasGroup 추가
    public Image deadImage;

    public Button returnButton;                 // 죽은 후 뜨는 버튼 연결
    public float fadeSpeed = 2f;

    private void Awake()
    {
        // 패널은 항상 Active 상태로
        deathPanelGroup.alpha = 0;
        deathPanelGroup.blocksRaycasts = false; // 클릭 막기

        if (deadImage != null)
        {
            deadImage.enabled = true;
        }

        if (returnButton != null)
        {
            returnButton.interactable = false;
            returnButton.gameObject.SetActive(false);
        }
    }

    public void PlayDeathSequence()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        deathPanelGroup.alpha = 0f;
        deathPanelGroup.blocksRaycasts = false;

        float timer = 0f;

        while (timer < 2f)
        {
            timer += Time.unscaledDeltaTime * fadeSpeed;
            float time = Mathf.Clamp01(timer / 1f);

            float eased = 0.5f - 0.5f * Mathf.Cos(Mathf.PI * time);
            deathPanelGroup.alpha = eased;

            if (deadImage != null)
            {
                deadImage.color = Color.Lerp(Color.clear, new Color(0.8f, 0, 0, 1), eased);
            }

            yield return null;
        }

        deathPanelGroup.alpha = 1f;
        deathPanelGroup.blocksRaycasts = true; // 마지막에 클릭 가능해짐

        if (deadImage != null)
        {
            deadImage.color = new Color(0.8f, 0, 0, 1);
        }

        if (returnButton != null)
        {
            returnButton.gameObject.SetActive(true);
            returnButton.interactable = true;
        }

        Time.timeScale = 0f;
    }
}
