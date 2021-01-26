using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MyEvents;
using System.IO;

using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public bool stopTimer = false;
    [SerializeField] private float turnDuration = 0f;                                     //How long does a turn last.
    [ShowOnly] private float turnTimer = 0f;                                              //Timer for every turn.

    [ShowOnly] public float timerToReduceTime = 0.0f;                                 //Timer to reduce the time between turns
    [SerializeField] private float timeToReduceTimeBetweenTurns = 0.0f;               //How much time does it take to reduce the time between turns.
    [SerializeField] private float TimeReduced = 0.0f;                                //How much time it will be reduced to the time between turns.
    [SerializeField] private float minTimeBetweeTurns = 0.0f;                         //The minimum time posible for the turns.

    [SerializeField] private float skyboxSaturation;
    [SerializeField] private float skyboxValue;
    private float skyboxRotateSpeed = 20f;

    [SerializeField] private GameObject buildingPrefab;

    private void Awake()
    {
        Managers.Turn = this;
    }

    private void Start()
    {
        turnTimer = turnDuration;
        stopTimer = false;
        GenerateBuildings();
    }

    public void GenerateBuildings()
    {
        int magnitude = 5;

        int topLimit = 0;
        int botLimit = magnitude;

        float tileMargin = 0f;

        Vector3 tileSize = buildingPrefab.GetComponentInChildren<Renderer>().bounds.size;
        float tileWidth = tileSize.z + tileMargin;
        float tileHeight = tileSize.x + tileMargin;

        for (int column = -magnitude; column <= magnitude; ++column)
        {
            for (int row = topLimit; row <= botLimit; ++row)
            {
                Vector2Int gridPosition = new Vector2Int(column, row);
                Vector3 worldPosition = Managers.Tiles.GridToWorld(gridPosition, tileHeight, tileWidth);
                worldPosition.y = -13.9f;

                GameObject instance = Instantiate(
                    buildingPrefab,
                    worldPosition,
                    Quaternion.Euler(0f, 90f, 0f));
            }
            if (column < 0)
            {
                --topLimit;
            }
            else
            {
                --botLimit;
            }
        }
    }

    void Update()
    {
        turnTimer -= Time.deltaTime;

        if (turnTimer <= 0f && !stopTimer)
        {
            //End of the turn
            
            // player.GetComponent<PlayerVars>().moving = false;
            if (PhotonNetwork.IsMasterClient)
            {
                //We get the MasterClient's PhotonView and call a function in all the other clients
                PhotonView photonView = Managers.Network.GetComponent<PhotonView>();
                photonView.RPC("TimerReset", RpcTarget.All);
            }
        }

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxRotateSpeed);

        //ReduceTurnDuration();
    }

    private void OnEnable()
    {
        EventManager.StartListening(MyEventType.PlayerReachGoal, StopTimer);
    }

    private void OnDisable()
    {
        EventManager.StopListening(MyEventType.PlayerReachGoal, StopTimer);
    }

    private void StopTimer(object info)
    {
        stopTimer = true;
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
        turnTimer = turnDuration;
        Color nextColor = GetRandomColorDifferentHue(RenderSettings.skybox.GetColor("_Tint"), 15f);
        RenderSettings.skybox.SetColor("_Tint", nextColor);
    }

    private Color GetRandomColorDifferentHue(Color prevColor, float minHueDifference)
    {
        float prevHue, prevSaturation, prevValue;
        Color.RGBToHSV(prevColor, out prevHue, out prevSaturation, out prevValue);
        float nextHue;
        do
        {
            nextHue = Random.Range(0f, 1f);
        } while (Mathf.Abs(nextHue - prevHue) < (minHueDifference / 360f));
        return Color.HSVToRGB(nextHue, skyboxSaturation, skyboxValue);
    }
}
