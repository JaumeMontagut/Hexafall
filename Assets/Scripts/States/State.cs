using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class State
{
    protected StateMachine stateMachine;
    public void SetStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    protected void ChangeState(State newState)
    {
        stateMachine.ChangeState(newState);
    }
    public void ChangeState(State nextState, float time)
    {
        stateMachine.ChangeState(nextState, time);
    }
}