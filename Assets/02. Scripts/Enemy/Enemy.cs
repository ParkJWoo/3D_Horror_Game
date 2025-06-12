using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field:SerializeField] public EnemySO Data { get; private set; }
    [field:SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public AudioSource AudioSource { get; private set; }

    public EnemyStateMachine stateMachine;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        AudioSource = GetComponent<AudioSource>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }
}
