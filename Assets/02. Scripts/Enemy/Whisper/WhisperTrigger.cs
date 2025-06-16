using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;


public class WhisperTrigger : MonoBehaviour
{
    public string soundName = "Whisper";

    [Header("Camera Effects")]
    public CinemachineVirtualCamera virtualCam;
    public float fovTarget = 40f;               //  시야각
    public float fovTransitionTime = 1f;

    [Header("Vignette Settings")]
    public Volume postProcessingVolume;
    public float vignetteTargetIntensity = 0.5f;
    public float vignetteTransitionTime = 1f;

    private float originalFov;
    private Coroutine fovCoroutine;
    private Coroutine vignetteCoroutine;

    private Vignette vignette;

    private bool hasPlayerSound = false;

    private void Start()
    {
        //  Volume에서 Vignette 가져오기
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }

        else
        {
            Debug.LogWarning("Vignette not found in Volume profile.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WhisperTrigger 진입 → 사운드 + 시야 축소 + 비네트");

            hasPlayerSound = true;

            SoundManager.Instance.PlayLoopSfx(soundName);

            //  Fov 축소
            if (virtualCam != null)
            {
                originalFov = virtualCam.m_Lens.FieldOfView;

                if(fovCoroutine != null)
                {
                    StopCoroutine(fovCoroutine);
                }

                fovCoroutine = StartCoroutine(ChangeFovCoroutine(virtualCam, fovTarget));
            }

            //  Vignette 강화
            if (vignette != null)
            {
                if(vignetteCoroutine != null)
                {
                    StopCoroutine(vignetteCoroutine);
                }

                vignetteCoroutine = StartCoroutine(ChangeVignetteCoroutine(vignette.intensity.value, vignetteTargetIntensity));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WhisperTrigger 이탈 → 사운드 중단 + 시야 복구 + 비네트 해제");

            hasPlayerSound = false;

            SoundManager.Instance.StopLoopSfx();

            //  Fov 복구
            if (virtualCam != null)
            {
                if (fovCoroutine != null)
                {
                    StopCoroutine(fovCoroutine);
                }

                fovCoroutine = StartCoroutine(ChangeFovCoroutine(virtualCam, originalFov));
            }

            //  Vignette 복구
            if (vignette != null)
            {
                if (vignetteCoroutine != null)
                {
                    StopCoroutine(vignetteCoroutine);
                }

                vignetteCoroutine = StartCoroutine(ChangeVignetteCoroutine(vignette.intensity.value, 0f));
            }
        }
    }

    private IEnumerator ChangeFovCoroutine(CinemachineVirtualCamera cam, float targetFov)
    {
        float startFov = cam.m_Lens.FieldOfView;
        float elapsed = 0f;

        while (elapsed < fovTransitionTime)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(startFov, targetFov, elapsed / fovTransitionTime);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cam.m_Lens.FieldOfView = targetFov;
    }

    private IEnumerator ChangeVignetteCoroutine(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < vignetteTransitionTime)
        {
            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(from, to, elapsed / vignetteTransitionTime);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (vignette != null)
        {
            vignette.intensity.value = to;
        }
    }
}
