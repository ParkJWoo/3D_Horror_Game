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
        stateMachine.Context.Agent.isStopped = false;
        stateMachine.Context.Agent.speed = stateMachine.Context.Data.WalkSpeed;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.WalkParameterHash, true);
    }

    public void Exit()
    {
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

        //  거리 계산
        float distance = Vector3.Distance(stateMachine.Context.PlayerTransform.position, stateMachine.Context.transform.position);

        //  공격 범위 진입 → 공격 상태 전이
        if (distance <= stateMachine.Context.Data.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        //  도망 거리를 넘으면 추적 중단
        if(distance > stateMachine.Context.Data.PlayerChasingRange)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        stateMachine.Context.Agent.SetDestination(stateMachine.Context.PlayerTransform.position);
    }
}
