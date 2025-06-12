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
        //stateMachine.MovementSpeedModifier = stateMachine.Context.Data.GroundData.WalkSpeedModifier;
        stateMachine.MovementSpeedModifier = 3f;
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
        //  이동 메서드 호출!
        MoveTowardTarget();

        // 상태 전이 체크
        float distSqr = (stateMachine.Target.transform.position - stateMachine.Context.transform.position).sqrMagnitude;

        float attackRangeSqr = Mathf.Pow(stateMachine.Context.Data.AttackRange, 2);

        if (distSqr <= attackRangeSqr)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }

        else if (distSqr > Mathf.Pow(stateMachine.Context.Data.PlayerChasingRange, 2))
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    //  실제 이동 처리
    private void MoveTowardTarget()
    {
        Vector3 targetPos = stateMachine.Target.transform.position;
        Vector3 myPos = stateMachine.Context.transform.position;

        targetPos.y = myPos.y;

        Vector3 dir = (targetPos - myPos).normalized;
        float moveSpeed = 3f; 

        Vector3 move = dir * moveSpeed * Time.deltaTime;

        stateMachine.Context.Controller.Move(move);

        // 자연스러운 회전을 추가하여 부드럽게 플레이어를 향하도록 구성
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            stateMachine.Context.transform.rotation = Quaternion.Slerp(stateMachine.Context.transform.rotation, targetRot, 10f * Time.deltaTime);
        }
    }
}
