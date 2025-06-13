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

        //  슬랜더맨 이동 멈춤
        stateMachine.Context.Agent.isStopped = true;

        //  공격 애니메이션 실행
        stateMachine.Context.Animator.SetTrigger(stateMachine.Context.AnimationData.ScreamParameterHash);

        //  사운드
        if(stateMachine.Context.AudioSource && stateMachine.Context.Data.ScreamClip)
        {
            stateMachine.Context.AudioSource.PlayOneShot(stateMachine.Context.Data.ScreamClip);
        }

        //  클로즈업 카메라 전환
        SetCloseupCameraPriority(30);

        //  공격 연출 코루틴 시작
        stateMachine.Context.StartCoroutine(AttackSequence());
    }

    public void Exit()
    {
        stateMachine.Context.Agent.isStopped = false;
        SetCloseupCameraPriority(5);                    //  연출 끝나면 복구
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
        //  연출 대기
        yield return new WaitForSeconds(stateMachine.Context.Data.Scream_End_TransitionTime);

        //  실제 사망 처리
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            stateMachine.Context.PlayerController.Die();
        }

        //  잠깐 대기 후 상태 복귀
        yield return new WaitForSeconds(1.0f);

        //  상태 복귀 / 카메라 Priority 복구는 Eixt에서 처리
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    //  클로즈업 카메라 Priority 변경 메서드 (공격 시 30, 평소 5)
    private void SetCloseupCameraPriority(int priority)
    {
        var cam = stateMachine.Context.CloseupCamera;

        if (cam != null)
        {
            cam.Priority = priority;
        }
    }

}
