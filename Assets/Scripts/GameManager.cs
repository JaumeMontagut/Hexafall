using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;
using Photon.Pun;
using System;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> players;
    bool finished = false;

    void Awake()
    {
        Managers.Game = this;

        //RoomManager is an object that comes from the MainMenuScene and doesn't destroy on the loading of the scene
        //We check if it exists to see if you started playing from the GameScene
        if (FindObjectOfType<RoomManager>() == null)
        {
            gameObject.AddComponent<DebugEnterRoom>();
        }
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        Action<dynamic> function = DisableMovementPlayers;
        EventManager.StartListening(MyEventType.PlayerReachGoal, DisableMovementPlayers);
    }

    private void OnDisable()
    {
        Action<dynamic> function = DisableMovementPlayers;
        EventManager.StopListening(MyEventType.PlayerReachGoal, function);
    }

    private void DisableMovementPlayers(dynamic info)
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

    //Works both when you stop playing in the editor and when you close the game in build
    private void OnApplicationQuit()
    {
        PhotonNetwork.LeaveRoom();
    }
}
