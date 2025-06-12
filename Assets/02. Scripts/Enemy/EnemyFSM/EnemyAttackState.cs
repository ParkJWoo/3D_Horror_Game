using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IState<Enemy>
{
    private EnemyStateMachine stateMachine;
    private bool alreadyAttacked;

    public EnemyAttackState(EnemyStateMachine sm)
    {
        stateMachine = sm;
    }

    public void Enter()
    {
        alreadyAttacked = false;
        stateMachine.Context.Agent.isStopped = true;
        stateMachine.Context.Animator.SetTrigger(stateMachine.Context.AnimationData.ScreamParameterHash);

        //  사운드
        if(stateMachine.Context.AudioSource && stateMachine.Context.Data.ScreamClip)
        {
            stateMachine.Context.AudioSource.PlayOneShot(stateMachine.Context.Data.ScreamClip);
        }

        stateMachine.Context.StartCoroutine(AttackSequence());
    }

    public void Exit()
    {
        stateMachine.Context.Agent.isStopped = false;
    }

    public void HandleInput()
    {
    }

    public void PhysicsUpdate()
    {
    }

    public void Update()
    {
    }

    private IEnumerator AttackSequence()
    {
        //  1. 카메라 클로즈업 연출
        if(stateMachine.Context.Data.CloseupCamera != null)
        {
            stateMachine.Context.Data.CloseupCamera.Priority = 20;
        }

        yield return new WaitForSeconds(stateMachine.Context.Data.Scream_End_TransitionTime);

        if(!alreadyAttacked)
        {
            stateMachine.Context.PlayerController.Die();
            alreadyAttacked = true;

            yield return new WaitForSeconds(1.0f);

            //  사망 UI 패널 생성은 PlayerController.cs Die 메서드에서!
            if(stateMachine.Context.Data.CloseupCamera != null)
            {
                stateMachine.Context.Data.CloseupCamera.Priority = 5;
            }
        }
    }

}
