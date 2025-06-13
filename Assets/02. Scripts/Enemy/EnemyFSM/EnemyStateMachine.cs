using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine<Enemy>
{
    public Enemy Context { get; }
    public EnemyIdleState IdleState { get; }
    public EnemyChasingState ChasingState { get; }
    public EnemyAttackState AttackState { get; }
    private IState<Enemy> currentState;

    public EnemyStateMachine(Enemy context)
    {
        Context = context;
        IdleState = new EnemyIdleState(this);
        ChasingState = new EnemyChasingState(this);
        AttackState = new EnemyAttackState(this);
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
