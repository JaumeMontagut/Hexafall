using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Used to send game-related data (not specific to any players)
//Each player has one

public class NetworkManager : MonoBehaviour
{
    private void Awake()
    {
        Managers.Network = this;
    }

    [PunRPC]
    public void TimerReset()
    {
        foreach (GameObject player in Managers.Game.players)
        {
            PlayerMove playerMovement = player.GetComponent<PlayerMove>();
            playerMovement.AvailableMovements = 1;
        }
        Managers.Turn.NewTurn();
    }
}
