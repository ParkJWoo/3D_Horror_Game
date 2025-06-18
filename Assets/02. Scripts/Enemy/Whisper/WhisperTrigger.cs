using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WhisperTrigger : MonoBehaviour
{
    [SerializeField] private WhisperEffectController effectController;

    private Transform playerTransform;
    private bool isPlayerInside = false;
    private bool hasTriggered = false;      //  한 번만 트리거 발동되게금

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isPlayerInside = true;

            effectController.BeginWhisperEffect(playerTransform);
            hasTriggered = true;
        }
    }

    private void Update()
    {
        if (isPlayerInside && !effectController.IsSuppressed && !effectController.IsPlaying)
        {
            effectController.BeginWhisperEffect(playerTransform);
            hasTriggered = true;
        }
    }

    //  수동 강제 트리거용 메서드
    public void ForceTrigger(Transform player)
    {
        if (!hasTriggered)
        {
            playerTransform = player;
            isPlayerInside = true;
            effectController.BeginWhisperEffect(playerTransform);
            hasTriggered = true;
        }
    }
}