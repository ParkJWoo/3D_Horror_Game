using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [field:SerializeField] public EnemySO Data { get; private set; }
    [field:SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }

    public Transform HeadTransform;
    public Transform CloseupCamPoint;

    public Cinemachine.CinemachineVirtualCamera CloseupCamera; //  공격 시 카메라 클로즈업용

    public Transform PlayerTransform { get; private set; }
    public PlayerController PlayerController { get; private set; }

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        AudioSource = GetComponent<AudioSource>();
        StateMachine = new EnemyStateMachine(this);
    }

    private void OnEnable()
    {
        if (CharacterManager.Instance?.Player?.controller != null)
        {
            PlayerController = CharacterManager.Instance.Player.controller;
            PlayerTransform = PlayerController.transform;
        }

        if (StateMachine != null && PlayerTransform != null)
        {
            if (StateMachine.IsForcedChase)
            {
                Debug.Log("슬렌더맨: 강제 추격 상태 진입");
                StateMachine.ChangeState(StateMachine.ChasingState);
            }
            else
            {
                StateMachine.ChangeState(StateMachine.IdleState);
            }
        }
    }

    private void Start()
    {
        PlayerController = CharacterManager.Instance.Player.controller;
        PlayerTransform = PlayerController.transform;
    }

    private void Update()
    {
        StateMachine.UpdateState();
    }
}
