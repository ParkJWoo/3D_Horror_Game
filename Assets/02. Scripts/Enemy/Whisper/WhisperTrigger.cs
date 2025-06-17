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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            isPlayerInside = true;

            effectController.BeginWhisperEffect(playerTransform);
        }
    }

    private void Update()
    {
        if (isPlayerInside && !effectController.IsSuppressed && !effectController.IsPlaying)
        {
            effectController.BeginWhisperEffect(playerTransform);
        }
    }
}