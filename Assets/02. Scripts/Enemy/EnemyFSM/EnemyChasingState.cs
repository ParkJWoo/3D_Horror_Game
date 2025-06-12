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
        //  �̵� �޼��� ȣ��!
        MoveTowardTarget();

        // ���� ���� üũ
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

    //  ���� �̵� ó��
    private void MoveTowardTarget()
    {
        Vector3 targetPos = stateMachine.Target.transform.position;
        Vector3 myPos = stateMachine.Context.transform.position;

        targetPos.y = myPos.y;

        Vector3 dir = (targetPos - myPos).normalized;
        float moveSpeed = 3f; 

        Vector3 move = dir * moveSpeed * Time.deltaTime;

        stateMachine.Context.Controller.Move(move);

        // �ڿ������� ȸ���� �߰��Ͽ� �ε巴�� �÷��̾ ���ϵ��� ����
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);

            stateMachine.Context.transform.rotation = Quaternion.Slerp(stateMachine.Context.transform.rotation, targetRot, 10f * Time.deltaTime);
        }
    }
}
