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

        //  공격 직전 슬랜더맨을 플레이어 쪽으로 강제 회전
        SetCloseupCameraPoint();

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

    private void SetCloseupCameraPoint()
    {
        var head = stateMachine.Context.HeadTransform;              //  제발 정면을 향해 바라봐주세요 ㅠㅠ
        var camPoint = stateMachine.Context.CloseupCamPoint;        //  공격 연출 카메라 포인트

        float camOffset = 0.8f;

        //  Head 정면 camOffset 만큼 앞에 카메라 포인트 위치
        Vector3 camPos = head.position + head.forward * camOffset;

        //  벽 Raycast 체크
        RaycastHit hit;
        Vector3 dir = head.forward;
        Vector3 desiredPos = camPos;

        if(Physics.Raycast(head.position, dir, out hit, camOffset, LayerMask.GetMask("Default")))
        {
            //  벽이 있으면 살짝 앞에 세움
            desiredPos = hit.point - dir * 0.05f;
        }

        camPoint.position = desiredPos;

        //  카메라가 Head를 바라보게 해주세요 ㅠㅠ
        camPoint.LookAt(head.position);
    }
}
