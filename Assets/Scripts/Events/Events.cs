using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEvents
{
    public enum MyEventType
    {
        #region Animation

        PlayerJumpTop,

        #endregion
        PlayerEndJump,
        #region Game

        StateWaitingEnter,
        StateWaitingExit,
        StateInGameEnter,
        StateInGameExit,
        StateResultsEnter,
        StateResultsExit,

        PlayerReachGoal,
        PlayerExploded,
        PlayerSelectInResults,

        #endregion

        #region Tiles


        #endregion


    }
}

