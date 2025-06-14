using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip [] footStepClips;
    private Rigidbody _rigidbody;
    public float footStepThreshold;
    public float footStepRate;
    private float footStepTime;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if(Mathf.Abs(_rigidbody.velocity.y)<0.1f)
        {
            if(_rigidbody.velocity.magnitude > footStepThreshold)
            {
                if(Time.time - footStepTime > footStepRate)
                {
                    footStepTime = Time.time;
                    //sound.SoundPlay(footStepClips[Random.Range(0, footStepClips.Length)]);
                }
            }
        }
    }
}
