using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;

public class ResultsState : State
{
    public override void Enter()
    {
        EventManager.StartListening(MyEventType.PlayerSelectInResults, PlayerSelectOption);
        EventManager.TriggerEvent(MyEventType.StateResultsEnter, null);

        // TODO(): Event pass player camera has to focus

    }
    public override void Exit()
    {
        EventManager.StopListening(MyEventType.PlayerSelectInResults, PlayerSelectOption);
        EventManager.TriggerEvent(MyEventType.StateResultsExit, null);
    }

    public void PlayerSelectOption(dynamic info )
    {
        string option = (string)info;
        if (option == "ReturnLobby")
        {
            // TODO(): Load Lobby Scene
        }
        else if (option == "ReturnMainMenu")
        {
            // TODO(): Load MainMenu Scene
        }
    }
}
