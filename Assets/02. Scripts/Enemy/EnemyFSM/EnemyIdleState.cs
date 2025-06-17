using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IState<Enemy>
{
    private EnemyStateMachine stateMachine;
    public EnemyIdleState(EnemyStateMachine sm)
    {
        stateMachine = sm;
    }

    public void Enter()
    {
        stateMachine.Context.Agent.isStopped = true;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.IdleParameterHash, true);
    }

    public void Exit()
    {
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.IdleParameterHash, false);
    }

    public void HandleInput()
    {
        throw new System.NotImplementedException();
    }

    public void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        if (stateMachine.Context.PlayerController.isDead)
        {
            return;
        }

        // 강제 추격 플래그가 켜져 있으면 즉시 추격 상태로 진입
        if (stateMachine.IsForcedChase)
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }

        // 일반 거리 기반 추격 로직 (필요하다면 유지)
        float distSqr = (stateMachine.Context.PlayerTransform.position - stateMachine.Context.transform.position).sqrMagnitude;
        float chaseRange = stateMachine.Context.Data.PlayerChasingRange;

        if (distSqr <= chaseRange * chaseRange)
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
    }

    //private bool IsInChasingRange()
    //{
    //    float distSqr = (stateMachine.Context.PlayerTransform.position - stateMachine.Context.transform.position).sqrMagnitude;

    //    float chaseRange = stateMachine.Context.Data.PlayerChasingRange;

    //    return distSqr <= chaseRange * chaseRange;
    //}
}
