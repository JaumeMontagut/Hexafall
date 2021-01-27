using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region GameData
    public float timeWaitingToStart;
    public Vector2Int startPosition;

    [HideInInspector] public const int mainMenu = 0;
    [SerializeField] private GameObject ButtonMainMenu, ButtonPlayAgain;
    [SerializeField] private StateMachine stateMachine;
    [HideInInspector] public List<GameObject> players;

    #endregion

    public Material GameSkybox;
    void Awake()
    {
        Managers.Game = this;
        RenderSettings.skybox = GameSkybox;

        //RoomManager is an object that comes from the MainMenuScene and doesn't destroy on the loading of the scene
        //We check if it exists to see if you started playing from the GameScene
        if (FindObjectOfType<RoomManager>() == null)
        {
            gameObject.AddComponent<DebugEnterRoom>();
        }
    }

    private void Start()
    {
        ButtonMainMenu.GetComponent<Button>().onClick.AddListener(GoToMainMenu);
        ButtonPlayAgain.GetComponent<Button>().onClick.AddListener(GoToRoom);



    }
    private void OnEnable()
    {
        EventManager.StartListening(MyEventType.PlayerReachGoal, WinResult);
        EventManager.StartListening(MyEventType.DesactivateInput, DisableMovementPlayers);

    }
    private void OnDisable()
    {
        EventManager.StopListening(MyEventType.PlayerReachGoal, WinResult);
        EventManager.StopListening(MyEventType.DesactivateInput, DisableMovementPlayers);

    }

    public void StartStateMachine()
    {
        stateMachine.ChangeState(new WaitingState());
    }
    private void WinResult(object info)
    {
        EventManager.TriggerEvent(MyEventType.DesactivateInput, info);
    }
    private void DisableMovementPlayers(object info)
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerMove>().AvailableMovements = 0;
        }
    }
    public PhotonView GetCurrentPhotonView()
    {
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (PhotonNetwork.LocalPlayer.ActorNumber == photonView.OwnerActorNr)
            {
                return photonView;
            }
        }
        Debug.LogError("Local player Photon view not found.");
        return null;
    }

    public GameObject GetCurrentPlayer()
    {
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (PhotonNetwork.LocalPlayer.ActorNumber == photonView.OwnerActorNr)
            {
                return player;
            }
        }
        Debug.LogError("Local player not found.");
        return null;
    }

    private void GoToMainMenu()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LoadLevel(mainMenu);
        PhotonNetwork.LeaveRoom();
    }

    private void GoToRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LoadLevel(mainMenu);
    }
}

