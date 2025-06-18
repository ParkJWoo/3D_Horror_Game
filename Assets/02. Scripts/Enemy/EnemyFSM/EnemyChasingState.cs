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
        // 이동 시작 설정, 속도는 이미 MapController에서 세팅
        stateMachine.Context.Agent.isStopped = false;
        // stateMachine.Context.Agent.speed = ... (여기서는 X)
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.WalkParameterHash, true);

        SoundManager.Instance.SwitchBgm("ChaseBGM");
        SoundManager.Instance.PlayLoopEnemySound("Growling");
    }

    public void Exit()
    {
        SoundManager.Instance.StopLoopEnemySound();
        stateMachine.Context.Agent.isStopped = true;
        stateMachine.Context.Animator.SetBool(stateMachine.Context.AnimationData.WalkParameterHash, false);
    }

    public void HandleInput() { }
    public void PhysicsUpdate() { }
    public void Update()
    {
        if (stateMachine.Context.PlayerController.isDead)
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

        stateMachine.Context.Agent.SetDestination(stateMachine.Context.PlayerTransform.position);
    }
}