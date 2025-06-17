using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChasingState : IState<Enemy>
{
    private EnemyStateMachine stateMachine;
    
    public EnemyChasingState(EnemyStateMachine sm)
    {
        stateMachine = sm;
    }

    public void Enter()
    {
        //  이동 시작 설정
        stateMachine.Context.Agent.isStopped = false;
        stateMachine.Context.Agent.speed = stateMachine.Context.Data.WalkSpeed;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.WalkParameterHash, true);

        //  슬랜더맨 추격 시작 사운드
        SoundManager.Instance.SwitchBgm("ChaseBGM");
        SoundManager.Instance.PlayLoopEnemySound("Growling");
    }

    public void Exit()
    {
        //  효과음 정지 및 이동 정지
        SoundManager.Instance.StopLoopEnemySound();
        stateMachine.Context.Agent.isStopped = true;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.WalkParameterHash, false);
    }

    public void HandleInput()
    {
    }

    public void PhysicsUpdate()
    {
    }

    public void Update()
    {
        //  플레이어가 죽었으면 Idle 상태로 전환
        if(stateMachine.Context.PlayerController.isDead)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        //  공격 범위 진입
        float distance = Vector3.Distance(stateMachine.Context.PlayerTransform.position, stateMachine.Context.transform.position);

        if (distance <= stateMachine.Context.Data.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        //  항상 플레이어를 추격
        stateMachine.Context.Agent.SetDestination(stateMachine.Context.PlayerTransform.position);
    }
}
