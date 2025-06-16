using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    public float vignetteIntensifyOnFail = 0.7f;

    [Header("Slenderman Settings")]
    public GameObject slenderman;
    public float whisperTimeBeforeSpawn = 10f;
    public float spawnDistance = 5f;
    public float minAngle = 60f;
    public float maxAngle = 120f;

    private float originalFov;
    private Coroutine fovCoroutine;
    private Coroutine vignetteCoroutine;
    private Coroutine whisperTimerCoroutine;

    private Vignette vignette;
    private bool isPlayerInside = false;
    private bool whisperSuppressed = false;
    private bool isWhisperPlaying = false;

    private Transform playerTransform;

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
            if (fovCoroutine != null) StopCoroutine(fovCoroutine);
            fovCoroutine = StartCoroutine(ChangeFovCoroutine(virtualCam, originalFov));
        }

        if (vignette != null)
        {
            if (vignetteCoroutine != null) StopCoroutine(vignetteCoroutine);
            vignetteCoroutine = StartCoroutine(ChangeVignetteCoroutine(vignette.intensity.value, 0f));
        }

        if (whisperTimerCoroutine != null)
        {
            StopCoroutine(whisperTimerCoroutine);
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

            playerTransform = other.transform;

            SoundManager.Instance.PlayLoopSfx(soundName);
            isWhisperPlaying = true;

            if (virtualCam != null)
            {
                originalFov = virtualCam.m_Lens.FieldOfView;
                if (fovCoroutine != null) StopCoroutine(fovCoroutine);
                fovCoroutine = StartCoroutine(ChangeFovCoroutine(virtualCam, fovTarget));
            }

            if (vignette != null)
            {
                if (vignetteCoroutine != null) StopCoroutine(vignetteCoroutine);
                vignetteCoroutine = StartCoroutine(ChangeVignetteCoroutine(vignette.intensity.value, vignetteTargetIntensity));
            }

            if (whisperTimerCoroutine != null) StopCoroutine(whisperTimerCoroutine);
            whisperTimerCoroutine = StartCoroutine(WhisperTimer());
        }
    }

    private IEnumerator WhisperTimer()
    {
        yield return new WaitForSeconds(whisperTimeBeforeSpawn);

        if (!whisperSuppressed && playerTransform != null)
        {
            Debug.Log("시간 초과 → 슬렌더맨 등장");

            if (vignette != null)
            {
                if (vignetteCoroutine != null) StopCoroutine(vignetteCoroutine);
                vignetteCoroutine = StartCoroutine(ChangeVignetteCoroutine(vignette.intensity.value, vignetteIntensifyOnFail));
            }

            if (slenderman != null)
            {
                Vector3 playerPos = playerTransform.position;
                Vector3 forward = playerTransform.forward;
                float angle = Random.Range(minAngle, maxAngle);
                if (Random.value < 0.5f) angle = -angle;

                Vector3 spawnDir = Quaternion.Euler(0, angle, 0) * forward;
                Vector3 spawnPos = playerPos + spawnDir.normalized * spawnDistance;

                slenderman.transform.position = spawnPos;

                Vector3 lookAtPos = new Vector3(playerPos.x, slenderman.transform.position.y, playerPos.z);
                slenderman.transform.LookAt(lookAtPos);

                slenderman.SetActive(true);

                var enemy = slenderman.GetComponent<Enemy>();
                if (enemy != null && enemy.StateMachine != null)
                {
                    enemy.StateMachine.ChangeState(enemy.StateMachine.ChasingState);
                }
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
