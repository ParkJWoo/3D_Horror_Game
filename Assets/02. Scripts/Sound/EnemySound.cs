using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private SoundController sound;
    private AudioClip crowlingClip;

    private void Awake()
    {
        sound = GetComponent<SoundController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
