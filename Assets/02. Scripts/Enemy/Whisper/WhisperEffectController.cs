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
    public float minWhisperTimeBeforeSpawn = 8f;            //  최소 등장 대기 시간
    public float maxWhisperTimeBeforeSpawn = 15f;           //  최대 등장 대기 시간
    public float restartDelay = 10f;                        //  속삭임 억제 후 다시 재시작까지의 시간

    //  외부 접근을 위한 상태 공개 프로퍼티
    public bool IsSuppressed => suppressed;
    public bool IsPlaying => playing;

    private Vignette vignette;
    private Coroutine fovCoroutine, vignetteCoroutine, timerCoroutine, restartCoroutine;

    private bool suppressed = false;                        //  현재 속삭임이 억제된 상태인지
    private bool playing = false;                           //  현재 속삭임이 재생 중인지
    private Transform player;


    //  포스트 프로세싱 프로필에서 비네트 효과를 가져온다
    private void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }
    }

    //  속삭임 이펙트 시작
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

    //  속삭임 효과 종료 및 슬랜더맨 비활성화 처리
    public void SuppressWhisper()
    {
        suppressed = true;
        playing = false;

        //  속삭임 사운드 정지
        SoundManager.Instance.StopLoopSfx();

        //  원래 BGM 재생
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

    //  일정 시간이 지나면 슬랜더맨 등장
    private IEnumerator StartWhisperTimer()
    {
        float waitTime = Random.Range(minWhisperTimeBeforeSpawn, maxWhisperTimeBeforeSpawn);

        yield return new WaitForSeconds(waitTime);

        if (!suppressed && player != null)
        {
            //  비네트 강도 증가
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

    //  일정 시간이 지나면 속삭임을 다시 활성화 가능 상태로 전환
    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        suppressed = false;
        playing = false;
    }

    //  카메라 시야각 변경
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

    //  비네트 효과 변경
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
