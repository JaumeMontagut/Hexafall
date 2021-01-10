using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class HexafallLauncher : MonoBehaviourPunCallbacks
{
    public static HexafallLauncher Instance;

    [SerializeField] InputField roomInputField;
    
    //ErrorMenu
    [SerializeField] Text errorText;

    //RoomMenu
    [SerializeField] Text roomText;

    //FindRoomMenu
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;

    private void Awake()
    {
        Instance = this;
    }

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
        roomText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("ErrorMenu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("LoadingMenu");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("LoadingMenu");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("TitleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform roomButton in roomListContent)
        {
            Destroy(roomButton.gameObject);
        }

        foreach (RoomInfo room in roomList)
        {
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
        }
    }
}
