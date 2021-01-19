using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurnManager : MonoBehaviour
{
    [ShowOnly] public float timerTurn = 0.0f;                                         //Timer for every turn.
    [SerializeField] private float timeBetweenTurns = 0.0f;                           //How long does a turn last.

    [ShowOnly] public float timerToReduceTime = 0.0f;                                 //Timer to reduce the time between turns
    [SerializeField] private float timeToReduceTimeBetweenTurns = 0.0f;               //How much time does it take to reduce the time between turns.
    [SerializeField] private float TimeReduced = 0.0f;                                //How much time it will be reduced to the time between turns.
    [SerializeField] private float minTimeBetweeTurns = 0.0f;                         //The minimum time posible for the turns.

    // Update is called once per frame
    void Update()
    {
        timerTurn += Time.deltaTime;

        if (timerTurn >= timeBetweenTurns)
        {
            //End of the turn
           // player.GetComponent<PlayerVars>().moving = false;


            //reset timer
            timerTurn = 0.0f;
        }

        if(timeBetweenTurns > minTimeBetweeTurns) //Be carefully to not change the time between turns when a turn is not finished yet.
        {
            timerToReduceTime += Time.deltaTime;

            if (timeToReduceTimeBetweenTurns < timerToReduceTime)
            {
                //Rdeuce the time between turns.
                timeBetweenTurns -= TimeReduced;

                //Reset timer.
                timerToReduceTime = 0.0f;

            }
        }
    }
}
