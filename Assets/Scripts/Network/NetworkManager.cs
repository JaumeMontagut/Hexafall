using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//Used to send game-related data (not specific to any players)
//Each player has one

public class NetworkManager : MonoBehaviourPunCallbacks
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
