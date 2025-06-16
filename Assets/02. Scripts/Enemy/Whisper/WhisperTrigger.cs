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
    public float fovTarget = 40f;
    public float fovTransitionTime = 1f;

    [Header("Vignette Settings")]
    public Volume postProcessingVolume;
    public float vignetteTargetIntensity = 0.5f;
    public float vignetteTransitionTime = 1f;

    private float originalFov;
    private Coroutine fovCoroutine;
    private Coroutine vignetteCoroutine;

    private Vignette vignette;
    private bool isPlayerInside = false;
    private bool whisperSuppressed = false;
    private bool isWhisperPlaying = false;

    private void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }

        else
        {
            Debug.LogWarning("Vignette not found in Volume profile.");
        }
    }

    private void Update()
    {
        if (isPlayerInside && !whisperSuppressed && !isWhisperPlaying)
        {
            SoundManager.Instance.PlayLoopSfx(soundName);
            isWhisperPlaying = true;
        }
    }

    public void SuppressWhisperSound()
    {
        whisperSuppressed = true;
        isWhisperPlaying = false;
        SoundManager.Instance.StopLoopSfx();

        if (virtualCam != null)
        {
            if (fovCoroutine != null)
            {
                StopCoroutine(fovCoroutine);
            }

            fovCoroutine = StartCoroutine(ChangeFovCoroutine(virtualCam, originalFov));
        }

        if (vignette != null)
        {
            if (vignetteCoroutine != null)
            {
                StopCoroutine(vignetteCoroutine);
            }

            vignetteCoroutine = StartCoroutine(ChangeVignetteCoroutine(vignette.intensity.value, 0f));
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WhisperTrigger 진입 → 사운드 + 시야 축소 + 비네트");

            isPlayerInside = true;
            whisperSuppressed = false;
            isWhisperPlaying = false;

            SoundManager.Instance.PlayLoopSfx(soundName);
            isWhisperPlaying = true;

            if (virtualCam != null)
            {
                originalFov = virtualCam.m_Lens.FieldOfView;

                if (fovCoroutine != null)
                {
                    StopCoroutine(fovCoroutine);
                }

                fovCoroutine = StartCoroutine(ChangeFovCoroutine(virtualCam, fovTarget));
            }

            if (vignette != null)
            {
                if (vignetteCoroutine != null)
                {
                    StopCoroutine(vignetteCoroutine);
                }

                vignetteCoroutine = StartCoroutine(ChangeVignetteCoroutine(vignette.intensity.value, vignetteTargetIntensity));
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
