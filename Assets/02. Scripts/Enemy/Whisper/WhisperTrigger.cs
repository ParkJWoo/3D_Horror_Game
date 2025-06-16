using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WhisperTrigger : MonoBehaviour
{
    [SerializeField] private WhisperEffectController effectController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            effectController.BeginWhisperEffect(other.transform);
        }
    }
}