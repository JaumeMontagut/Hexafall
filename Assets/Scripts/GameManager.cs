using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> players;

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

        //EventManager.TriggerEvent(MyEventType.PlayerReachGoal, null);      //don't know how to use srry.
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerVars>().currentPlatform == Managers.Tiles.end)
            {
                //This player wins!!

                //Desactivate game flow
                Managers.Turn.stopTimer = true;
                foreach (GameObject player2 in players)
                {
                    player2.GetComponent<PlayerMove>().AvailableMovements = 0;
                }

                //Move Camera
                GameObject camera = GameObject.Find("Main Camera");

                GameObject endPlatform = Managers.Tiles.end.gameObject;
                camera.transform.position = new Vector3(endPlatform.transform.position.x + 2, endPlatform.transform.position.y + 0.5f, endPlatform.transform.position.z);
                camera.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
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
