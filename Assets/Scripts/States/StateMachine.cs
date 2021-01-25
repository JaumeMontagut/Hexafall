using UnityEngine;
using System.Collections;
public class StateMachine : MonoBehaviour
{
    [HideInInspector] public State currentState { get; private set; }
    [HideInInspector] public State nextState { get; private set; }

    public void Initialize(State initState)
    {
        if (initState == null)
        {
            Debug.Log(this.ToString() + " cannot Initialize; initState is null");
            return;
        }

        currentState = initState;
        currentState.Enter();
    }

    public void Finish()
    {
        if (currentState == null)
        {
            Debug.Log(this.ToString() + " cannot Finish; currentState is null");
            return;
        }

        StopCoroutine("UpdateState");
        currentState.Exit();
        currentState = null;
    }
    public void Update ()
    {
        if (currentState == null)
            return;
        currentState.Update();
    }
    public void ChangeState(State newState)
    {
        Finish();
        Initialize(newState);
    }
    public void ChangeState(State nextState, float time)
    {
        if (nextState == null)
        {
            Debug.Log(this.ToString() + " cannot ChangeStateDelayed; nextState is null");
            return;
        }

        this.nextState = nextState;
        Invoke("ChangeStateD", time);
    }

    private void ChangeStateD ()
    {
        Finish();
        Initialize(nextState);
        nextState = null;
    }

}

