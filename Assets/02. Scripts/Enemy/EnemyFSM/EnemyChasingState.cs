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
        if(stateMachine.Context.PlayerController.isDead)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        float distance = Vector3.Distance(stateMachine.Context.PlayerTransform.position, stateMachine.Context.transform.position);

        if (distance <= stateMachine.Context.Data.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        if(distance > stateMachine.Context.Data.PlayerChasingRange)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        stateMachine.Context.Agent.SetDestination(stateMachine.Context.PlayerTransform.position);
    }
}
