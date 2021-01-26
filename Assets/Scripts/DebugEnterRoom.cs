using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class DebugEnterRoom : MonoBehaviourPunCallbacks
{
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want

    private void Start()
    {
        Debug.Log("Connecting to mater");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Joined master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        PhotonNetwork.CreateRoom(GetRandomString(6));
    }

    private string GetRandomString(int charAmount)
    {
        string myString = "";
        for (int i = 0; i < charAmount; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }
        return myString;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room.");
        Debug.Log("Ready to play");

        if (!Managers.Tiles.hasSetupPath)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ClientManager"), Vector3.zero, Quaternion.identity);

            Managers.Tiles.GenerateTiles();
            List<int> pathId = PathGenerator.GeneratePath();
            Managers.Tiles.photonView.RPC("SetPath", RpcTarget.AllBuffered, pathId[0], pathId[pathId.Count - 1], pathId.ToArray());
            PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "Star"), Managers.Tiles.end.transform.position, Quaternion.identity);
            Managers.Network.SpawnPlayers();

        }
    }
}
