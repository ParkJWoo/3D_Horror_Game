using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IState<Enemy>
{
    private EnemyStateMachine stateMachine;
    private bool alreadyScreamed;

    public EnemyAttackState(EnemyStateMachine sm)
    {
        stateMachine = sm;
    }
    
    public void Enter()
    {
        Debug.Log("공격 시작!");
        alreadyScreamed = false;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.ScreamParameterHash, true);

        //스크림 사운드 재생
        if (stateMachine.Context.AudioSource && stateMachine.Context.Data.ScreamClip)
        {
            stateMachine.Context.AudioSource.PlayOneShot(stateMachine.Context.Data.ScreamClip);
        }
    }

    public void Exit()
    {
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.ScreamParameterHash, false);
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

        float normalizedTime = stateMachine.Context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (!alreadyScreamed && normalizedTime >= stateMachine.Context.Data.Scream_End_TransitionTime)
        {
            stateMachine.Target.Kill();

            //  공포 연출: 화면 흔들기, 이펙트 등 여기서 추가할 것
            alreadyScreamed = true;
        }

        if(normalizedTime >= 1f)
        {
            float distSqr = (stateMachine.Target.transform.position - stateMachine.Context.transform.position).sqrMagnitude;

            if(distSqr <= Mathf.Pow(stateMachine.Context.Data.AttackRange, 2))
            {
                stateMachine.ChangeState(stateMachine.AttackState);
            }

            else if(distSqr <= Mathf.Pow(stateMachine.Context.Data.PlayerChasingRange, 2))
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
            }

            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }
}
