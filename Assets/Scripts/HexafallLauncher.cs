using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HexafallLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField roomInputField;
    
    //ErrorMenu
    [SerializeField] Text errorText;

    private void Start()
    {
        Debug.Log("Connecting to Master 1");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master 2");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        MenuManager.Instance.OpenMenu("TitleMenu");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomInputField.text))
        {
            Debug.LogError("Invalid room name.");
            return;
        }
        PhotonNetwork.CreateRoom(roomInputField.text);
        MenuManager.Instance.OpenMenu("LoadingMenu");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("RoomMenu");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("ErrorMenu");
    }
}
