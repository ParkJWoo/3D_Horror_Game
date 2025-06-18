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

    // 쿨타임 조절용 필드
    public float baseMinWhisperTime = 8f;                // 최소 대기시간(초)
    public float baseMaxWhisperTime = 15f;               // 최대 대기시간(초)
    public float whisperTimeDecreasePerStage = 1.5f;     // 스테이지별 감소량(초)
    public float whisperMinLimit = 2f;                   // 최소 대기 하한

    public float restartDelay = 10f;                     // suppress 후 재시작 대기시간

    [Header("Map Control")]
    public MapController mapController;                  // ★ 인스펙터에서 직접 할당

    // 상태 공개
    public bool IsSuppressed => suppressed;
    public bool IsPlaying => playing;

    private Vignette vignette;
    private Coroutine fovCoroutine, vignetteCoroutine, timerCoroutine, restartCoroutine;

    private bool suppressed = false;
    private bool playing = false;
    private Transform player;

    private void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out Vignette v))
            vignette = v;

        // 인스펙터에서 안 넣었으면 찾아서 할당(최초 1회만)
        if (mapController == null)
            mapController = FindObjectOfType<MapController>();
    }

    // 속삭임 효과 시작
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

    // 속삭임 억제 (슬랜더맨 비활성화)
    public void SuppressWhisper()
    {
        suppressed = true;
        playing = false;

        SoundManager.Instance.StopLoopSfx();
        SoundManager.Instance.PlayBgmLoop("DefaultBGM");

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

        slendermanSpawner?.DeactivateSlenderman();

        if (restartCoroutine != null)
        {
            StopCoroutine(restartCoroutine);
        }

        restartCoroutine = StartCoroutine(RestartAfterDelay());
    }

    // Whisper Timer (스테이지별로 쿨타임 계산)
    private IEnumerator StartWhisperTimer()
    {
        // mapController에서 currentdataindex 안전하게 가져오기
        int stage = 0;
        if (mapController != null)
        {
            stage = mapController.currentdataindex;
        }

        float minTime = Mathf.Max(baseMinWhisperTime - whisperTimeDecreasePerStage * stage, whisperMinLimit);
        float maxTime = Mathf.Max(baseMaxWhisperTime - whisperTimeDecreasePerStage * stage, whisperMinLimit + 1f);

        float waitTime = Random.Range(minTime, maxTime);

        yield return new WaitForSeconds(waitTime);

        if (!suppressed && player != null)
        {
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

    // 억제 이후 효과 재시작
    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        suppressed = false;
        playing = false;
    }

    // 카메라 FOV 보간
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

    // Vignette 보간
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
