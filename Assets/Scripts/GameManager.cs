using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> players;
    
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
        EventManager.TriggerEvent(MyEventType.PlayerReachGoal, null);
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
