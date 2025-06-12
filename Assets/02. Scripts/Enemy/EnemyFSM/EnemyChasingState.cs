using System.Collections;
using System.Collections.Generic;
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
        //stateMachine.MovementSpeedModifier = stateMachine.Context.Data.GroundData.WalkSpeedModifier;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.WalkParameterHash, true);
    }

    public void Exit()
    {
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
        if(stateMachine.Target.IsDead)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        float distSqr = (stateMachine.Target.transform.position - stateMachine.Context.transform.position).sqrMagnitude;

        if(distSqr <= Mathf.Pow(stateMachine.Context.Data.AttackRange, 2))
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }

        else if(distSqr > Mathf.Pow(stateMachine.Context.Data.PlayerChasingRange, 2))
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
