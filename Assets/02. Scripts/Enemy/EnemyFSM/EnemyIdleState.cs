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
        stateMachine.MovementSpeedModifier = 0f;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.IdleParameterHash, true);
    }

    public void Exit()
    {
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.IdleParameterHash, false);
    }

    public void HandleInput()
    {
    }

    public void PhysicsUpdate()
    {
    }

    public void Update()
    {
        if(!stateMachine.Target.IsDead && IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
    }

    private bool IsInChasingRange()
    {
        float distSqr = (stateMachine.Target.transform.position - stateMachine.Context.transform.position).sqrMagnitude;
        return distSqr <= stateMachine.Context.Data.PlayerChasingRange * stateMachine.Context.Data.PlayerChasingRange;
    }
}
