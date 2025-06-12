using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void Enter();
    void Exit();
    void HandleInput();
    void Update();
    void PhysicsUpdate();
}

public abstract class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
{
    public IState<T> CurrentState { get; private set; }
    public T Context { get; private set; }

    protected virtual void Awake()
    {
        Context = GetComponent<T>();
    }

    public void ChangeState(IState<T> newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void HandleInput() => CurrentState?.HandleInput();
    public void UpdateState() => CurrentState?.Update();
    public void PhysicsUpdateState() => CurrentState?.PhysicsUpdate();
}
