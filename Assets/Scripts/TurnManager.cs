using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private float turnDuration = 0f;                                     //How long does a turn last.
    [ShowOnly] private float turnTimer = 0f;                                              //Timer for every turn.

    [ShowOnly] public float timerToReduceTime = 0.0f;                                 //Timer to reduce the time between turns
    [SerializeField] private float timeToReduceTimeBetweenTurns = 0.0f;               //How much time does it take to reduce the time between turns.
    [SerializeField] private float TimeReduced = 0.0f;                                //How much time it will be reduced to the time between turns.
    [SerializeField] private float minTimeBetweeTurns = 0.0f;                         //The minimum time posible for the turns.

    [SerializeField] private GameObject timerUI;
    private RectTransform timerPlane1;
    private RectTransform timerPlane2;
    private RectTransform foregroundPlane;

    private void Start()
    {
        turnTimer = turnDuration;

        timerPlane1 = timerUI.transform.Find("Plane1").GetComponent<RectTransform>();
        timerPlane2 = timerUI.transform.Find("Plane2").GetComponent<RectTransform>();

        foregroundPlane = timerPlane2;
    }

    public float TurnTimer
    {
        get
        {
            return turnTimer;
        }
        set
        {
            turnTimer = value;
            foregroundPlane.localScale = new Vector3(Mathf.Max(turnTimer / turnDuration, 0f), 1f, 1f);
        }
    }

    private void Awake()
    {
        Managers.Turn = this;
    }

    void Update()
    {
        TurnTimer -= Time.deltaTime;

        if (TurnTimer <= 0f)
        {
            //End of the turn
            
            // player.GetComponent<PlayerVars>().moving = false;

            if (Managers.Game.playLocal)
            {
                Managers.Network.TimerReset();
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                //We get the MasterClient's PhotonView and call a function in all the other clients
                PhotonView photonView = Managers.Network.GetComponent<PhotonView>();
                photonView.RPC("TimerReset", RpcTarget.All);
            }
        }

        //ReduceTurnDuration();
    }

    void ReduceTurnDuration()
    {
        if (turnDuration > minTimeBetweeTurns) //Be carefully to not change the time between turns when a turn is not finished yet.
        {
            timerToReduceTime += Time.deltaTime;

            if (timeToReduceTimeBetweenTurns < timerToReduceTime)
            {
                //Rdeuce the time between turns.
                turnDuration -= TimeReduced;

                //Reset timer.
                timerToReduceTime = 0.0f;
            }
        }
    }

    public void NewTurn()
    {
        Debug.Log("New turn time");
        TurnTimer = turnDuration;

        //Change the planes
        foregroundPlane.transform.SetAsFirstSibling();
        SelectBackgroundColor();
        foregroundPlane.localScale = Vector3.one;
        foregroundPlane = (foregroundPlane == timerPlane1 ? timerPlane2 : timerPlane1);
    }

    private void SelectBackgroundColor()
    {
        Color nextForegroundColor = (foregroundPlane == timerPlane1 ? timerPlane1 : timerPlane2).GetComponent<Image>().color;
        float nextForegroundHue, nextForegroundSaturation, nextForegroundValue;
        Color.RGBToHSV(nextForegroundColor, out nextForegroundHue, out nextForegroundSaturation, out nextForegroundValue);
        float backgroundHue;
        const float minHueDifference = 15f;
        do
        {
            backgroundHue = Random.Range(0f, 1f);
        } while (Mathf.Abs(backgroundHue - nextForegroundHue) < (minHueDifference / 360f));
        foregroundPlane.GetComponent<Image>().color = Color.HSVToRGB(backgroundHue, 1f, 1f);
    }
}
