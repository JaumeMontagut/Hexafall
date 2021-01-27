using MyEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingState : State
{
    public override void Enter()
    {
        EventManager.TriggerEvent(MyEventType.StateWaitingEnter, null);
        Managers.Audio.PlayAudio("Ready");
        ChangeState(new InGameState(), Managers.Game.timeWaitingToStart);
    }
    public override void Exit()
    {
        EventManager.TriggerEvent(MyEventType.StateWaitingExit, null);
    }
}
