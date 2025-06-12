using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Enemy : MonoBehaviour
{
    [field:SerializeField] public EnemySO Data { get; private set; }
    [field:SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }

    public Transform PlayerTransform { get; private set; }
    public PlayerController PlayerController { get; private set; }

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        AudioSource = GetComponent<AudioSource>();
        StateMachine = new EnemyStateMachine(this);

        PlayerController = CharacterManager.Instance.Player.controller;
        PlayerTransform = PlayerController.transform;
    }

    private void Start()
    {
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void Update()
    {
        StateMachine.UpdateState();
    }
}
