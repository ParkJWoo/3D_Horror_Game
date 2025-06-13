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
        if(!stateMachine.Context.PlayerController.isDead && IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
    }

    private bool IsInChasingRange()
    {
        float distSqr = (stateMachine.Context.PlayerTransform.position - stateMachine.Context.transform.position).sqrMagnitude;

        float chaseRange = stateMachine.Context.Data.PlayerChasingRange;

        return distSqr <= chaseRange * chaseRange;
    }
}
