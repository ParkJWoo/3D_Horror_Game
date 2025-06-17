using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WhisperEffectController : MonoBehaviour
{
    [Header("Sound")]
    public string soundName = "Whisper";

    [Header("Camera Effects")]
    public CinemachineVirtualCamera virtualCam;
    public float fovTarget = 40f;
    public float fovTransitionTime = 1f;
    private float originalFov;

    [Header("Vignette")]
    public Volume postProcessingVolume;
    public float vignetteTargetIntensity = 0.5f;
    public float vignetteTransitionTime = 1f;
    public float vignetteIntensifyOnFail = 0.7f;

    [Header("Slenderman")]
    public SlendermanSpawner slendermanSpawner;
    public float whisperTimeBeforeSpawn = 10f;
    public float restartDelay = 10f;

    private Vignette vignette;
    private Coroutine fovCoroutine, vignetteCoroutine, timerCoroutine, restartCoroutine;

    private bool suppressed = false;
    private bool playing = false;
    private Transform player;

    private void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }
    }

    public void BeginWhisperEffect(Transform playerTransform)
    {
        player = playerTransform;
        suppressed = false;
        playing = false;

        SoundManager.Instance.PlayLoopSfx(soundName);
        playing = true;

        if (virtualCam != null)
        {
            originalFov = virtualCam.m_Lens.FieldOfView;

            if (fovCoroutine != null)
            {
                StopCoroutine(fovCoroutine);
            }

            fovCoroutine = StartCoroutine(ChangeFov(virtualCam, fovTarget));
        }

        if (vignette != null)
        {
            if (vignetteCoroutine != null)
            {
                StopCoroutine(vignetteCoroutine);
            }

            vignetteCoroutine = StartCoroutine(ChangeVignette(vignette.intensity.value, vignetteTargetIntensity));
        }

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(StartWhisperTimer());
    }

    public void SuppressWhisper()
    {
        suppressed = true;
        playing = false;

        SoundManager.Instance.StopLoopSfx();

        if (virtualCam != null)
        {
            if (fovCoroutine != null)
            {
                StopCoroutine(fovCoroutine);
            }

            fovCoroutine = StartCoroutine(ChangeFov(virtualCam, originalFov));
        }

        if (vignette != null)
        {
            if (vignetteCoroutine != null)
            {
                StopCoroutine(vignetteCoroutine);
            }

            vignetteCoroutine = StartCoroutine(ChangeVignette(vignette.intensity.value, 0f));
        }

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        if (restartCoroutine != null)
        {
            StopCoroutine(restartCoroutine);
        }

        restartCoroutine = StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator StartWhisperTimer()
    {
        yield return new WaitForSeconds(whisperTimeBeforeSpawn);

        if (!suppressed && player != null)
        {
            Debug.Log("슬랜더맨 등장 트리거");

            if (vignette != null)
            {
                if (vignetteCoroutine != null)
                {
                    StopCoroutine(vignetteCoroutine);
                }

                vignetteCoroutine = StartCoroutine(ChangeVignette(vignette.intensity.value, vignetteIntensifyOnFail));
            }

            if (slendermanSpawner != null)
            {
                slendermanSpawner.Spawn(player);
            }
        }
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        suppressed = false;
        playing = false;
    }

    private IEnumerator ChangeFov(CinemachineVirtualCamera cam, float target)
    {
        float start = cam.m_Lens.FieldOfView;
        float t = 0f;

        while (t < fovTransitionTime)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(start, target, t / fovTransitionTime);
            t += Time.deltaTime;
            yield return null;
        }

        cam.m_Lens.FieldOfView = target;
    }

    private IEnumerator ChangeVignette(float from, float to)
    {
        float t = 0f;

        while (t < vignetteTransitionTime)
        {
            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(from, to, t / vignetteTransitionTime);
            }

            t += Time.deltaTime;
            yield return null;
        }

        if (vignette != null)
        {
            vignette.intensity.value = to;
        }
    }

}
