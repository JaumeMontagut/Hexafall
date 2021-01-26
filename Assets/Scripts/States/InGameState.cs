using MyEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameState : State
{
    public override void Enter()
    {
        EventManager.StartListening(MyEventType.PlayerReachGoal, FinishGame);
        EventManager.TriggerEvent(MyEventType.StateInGameEnter, null);

        // TODO(): Event enable player input
    }
    public override void Exit()
    {
        // TODO(): Event disable player input

        EventManager.StopListening(MyEventType.PlayerReachGoal, FinishGame);
        EventManager.TriggerEvent(MyEventType.StateInGameExit, null);
    }

    public void FinishGame(object info)
    {
        ChangeState( new ResultsState());
    }

}
