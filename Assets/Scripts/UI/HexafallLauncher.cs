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
    [SerializeField] public Text roomText;
    [SerializeField] public GameObject playerListContent;
    [SerializeField] public GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    //FindRoomMenu
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;

    //Game Scene
    [HideInInspector] public const int gameScene = 1;//Matches the build number in the build panel
    [HideInInspector] public const int mainMenu = 0;//Matches the build number in the build panel

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("Connecting to Master 1");

        if(MenuManager.Instance.AllCanvasDesactivated())
        {
            MenuManager.Instance.OpenMenu("LoadingMenu");
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master 2");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        MenuManager.Instance.OpenMenu("TitleMenu");
        //TODO: Change it so players can add their own name
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(gameScene);
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

        foreach (Transform child in playerListContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Instantiate(playerListItemPrefab, playerListContent.transform).GetComponent<PlayerListItem>().SetUp(player);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
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
            if (!room.RemovedFromList)
            {
                Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent.transform).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
