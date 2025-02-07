﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

//Used to send game-related data (not specific to any players)
//Each player has one

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        Managers.Network = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Managers.Tiles.GenerateTiles();
            List<int> pathId = PathGenerator.GeneratePath();
            Managers.Tiles.photonView.RPC("SetPath", RpcTarget.AllBuffered, pathId[0], pathId[pathId.Count - 1], pathId.ToArray());
            PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Star"), Managers.Tiles.end.transform.position, Quaternion.identity);
            SpawnPlayers();
            photonView.RPC("StartGameStateMachine", RpcTarget.AllBuffered);
        }
    }

    public void EnablePlayersInput(bool enable)
    {
        photonView.RPC("EnablePlayerInput", RpcTarget.AllBuffered, enable);
    }

    public void PlayNetworkingAudio(string name)
    {
        photonView.RPC("PlayAudio", RpcTarget.AllBuffered, name);
    }

    [PunRPC]
    private void PlayAudio(string name)
    {
        Managers.Audio.PlayAudio(name);
    }

    [PunRPC]
    private void EnablePlayerInput(bool enable)
    {
        Managers.Game.GetCurrentPlayer().GetComponent<PlayerMove>().enableInput = enable;
    }

    [PunRPC]
    public void StartGameStateMachine()
    {
        Managers.Game.StartStateMachine();
    }
    public void SpawnPlayers()
    {
        ClientManager[] clients = FindObjectsOfType<ClientManager>();
        foreach (ClientManager client in clients)
        {
            client.photonView.RPC("SpawnPlayer", RpcTarget.AllBuffered);
        }

        Managers.Game.StartStateMachine();
    }

    [PunRPC]
    public void TimerReset()
    {
        //foreach (GameObject player in Managers.Game.players)
        //{
        //    PlayerMove playerMovement = player.GetComponent<PlayerMove>();
        //    playerMovement.AvailableMovements = 1;
        //}

        foreach (GameObject player in Managers.Game.players)
        {
            PhotonView playerPhotonView = player.GetComponent<PhotonView>();
            if (playerPhotonView.OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                GameObject selectedPlatform = player.GetComponent<SelectPlatform>().SelectedPlatform;
                if(selectedPlatform != null)
                {
                    playerPhotonView.RPC(
                  "StartMoving",
                  RpcTarget.All,
                  selectedPlatform.GetComponent<PhotonView>().ViewID);
                }
              
            }
        }

        Managers.Turn.NewTurn();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for (int i = Managers.Game.players.Count - 1; i >= 0; --i)
        {
            if (Managers.Game.players[i].GetComponent<PhotonView>().OwnerActorNr == otherPlayer.ActorNumber)
            {
                Managers.Game.players.RemoveAt(i);
            }
        }
    }
}
