using UnityEngine;
using System.Collections;
public class StateMachine : MonoBehaviour
{
    [HideInInspector] public State currentState { get; private set; }

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
        if (newState == null)
        {
            Debug.Log(this.ToString() + " cannot ChangeStateDelayed; nextState is null");
        }
        else
        {
            newState.SetStateMachine(this);
        }
 
        Finish();
        Initialize(newState);
    }
    public void ChangeState(State newState, float time)
    {
        if (newState == null)
        {
            Debug.Log(this.ToString() + " cannot ChangeStateDelayed; nextState is null");
            return;
        }

        StartCoroutine(ChangeStateDelayed(newState, time));
    }

    IEnumerator ChangeStateDelayed(State newState, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(newState);
    }
}

