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
    [HideInInspector] public List<GameObject> players;
    public StateMachine stateMachine;
    public float timeWaitingToStart;
    [HideInInspector] public const int mainMenu = 0;
    [SerializeField] private GameObject ButtonMainMenu, ButtonPlayAgain;

    #endregion

    void Awake()
    {
        Managers.Game = this;

        //RoomManager is an object that comes from the MainMenuScene and doesn't destroy on the loading of the scene
        //We check if it exists to see if you started playing from the GameScene
        if (FindObjectOfType<RoomManager>() == null)
        {
            gameObject.AddComponent<DebugEnterRoom>();
        }

        stateMachine.ChangeState(new WaitingState());
    }
    private void Start()
    {
        ButtonMainMenu.GetComponent<Button>().onClick.AddListener(GoToMainMenu);
        ButtonPlayAgain.GetComponent<Button>().onClick.AddListener(GoToRoom);
    }
    private void OnEnable()
    {
        EventManager.StartListening(MyEventType.PlayerReachGoal, DisableMovementPlayers);
    }

    private void OnDisable()
    {
        EventManager.StopListening(MyEventType.PlayerReachGoal, DisableMovementPlayers);
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
        PhotonNetwork.LoadLevel(mainMenu);
        PhotonNetwork.LeaveRoom();
    }

    private void GoToRoom()
    {
        PhotonNetwork.LoadLevel(mainMenu);
    }
}

