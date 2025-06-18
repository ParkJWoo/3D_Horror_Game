using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<Enemy>
{
    public Enemy Context { get; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyChasingState ChasingState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    private IState<Enemy> currentState;

    public bool IsForcedChase { get; private set; }

    public EnemyStateMachine(Enemy context)
    {
        Context = context;
        IdleState = new EnemyIdleState(this);
        ChasingState = new EnemyChasingState(this);
        AttackState = new EnemyAttackState(this);
    }

    public void StartForcedChase()
    {
        IsForcedChase = true;
    }

    public void StopForcedChase()
    {
        IsForcedChase = false;
    }

    public void ChangeState(IState<Enemy> newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateState()
    {
        currentState?.Update();
    }
}
