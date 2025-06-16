using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.Instance.PlayBgmLoop("DefaultBGM");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.SwitchBgm("ChaseBGM");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SoundManager.Instance.SwitchBgm("DefaultBGM");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //SoundManager.Instance.Play3DSound("Growling", enemy.position);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SoundManager.Instance.PlaySound("Excute");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            SoundManager.Instance.PlayLoopSfx("HeartBeat");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SoundManager.Instance.StopLoopSfx();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SoundManager.Instance.PlayRandomSound("Steps");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //SoundManager.Instance.Play3DSound("Growling",enemy.position, player, 15);
        }
    }
}
