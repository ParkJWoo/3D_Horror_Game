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

    //  강제 추격 여부 추가 
    public bool IsForcedChase { get; protected set; }

    protected virtual void Awake()
    {
        Context = GetComponent<T>();
    }

    public void ChangeState(IState<T> newState)
    {
        //Debug.Log($"[FSM] 상태 변경: {CurrentState} -> {newState}");
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void StartForcedChase()
    {
        IsForcedChase = true;
    }

    public void StopForcedChase()
    {
        IsForcedChase = false;
    }

    public void HandleInput() => CurrentState?.HandleInput();
    public void UpdateState() => CurrentState?.Update();
    public void PhysicsUpdateState() => CurrentState?.PhysicsUpdate();
}
