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
    [Tooltip("Check this bool if you're developing the game, using only one player to move around the squares")]
    public bool playLocal;

    void Awake()
    {
        Managers.Game = this;

        if (playLocal)
        {
            PhotonNetwork.OfflineMode = true;
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
}
