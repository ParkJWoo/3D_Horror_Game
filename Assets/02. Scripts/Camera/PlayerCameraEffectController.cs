using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerCameraEffectController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform slenderman;
    public Volume globalVolume;
    public float maxEffectDistance = 8f;
    public float maxNoise = 1f;

    private FilmGrain filmGrain;

    private void Start()
    {
        if(globalVolume == null)
        {
            Debug.LogError("globalVolume을 드래그&드롭 해주세요!");
            enabled = false;
            return;
        }

        //  FilmGrain override 가져오기
        if(!globalVolume.profile.TryGet(out filmGrain) || filmGrain == null)
        {
            Debug.LogError("Global Volume에 FilmGrain(Override)가 없습니다!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (player == null || slenderman == null || filmGrain == null)
        {
            return;
        }

        float distance = Vector3.Distance(player.position, slenderman.position);
        float t = Mathf.Clamp01(1f - distance / maxEffectDistance);

        filmGrain.intensity.value = Mathf.Lerp(0f, maxNoise, t);
    }
}
