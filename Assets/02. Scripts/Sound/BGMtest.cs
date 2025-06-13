using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMtest : MonoBehaviour
{
    private SoundController sound;
    [SerializeField]private AudioClip bgmClip;
    
    

    private void Start()
    {
        sound = GetComponent<SoundController>();
        sound.PlayLoop(bgmClip);
    }
    
}
