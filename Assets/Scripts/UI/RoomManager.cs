using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Realtime;
using System.Linq;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private void Awake()
    {
        if (!Instance)
        {

        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == HexafallLauncher.gameScene)
        {
            if(!ClientManagerExist())
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ClientManager"), Vector3.zero, Quaternion.identity);
        }

        else if (scene.buildIndex == HexafallLauncher.mainMenu)
        {
            MenuManager.Instance.OpenMenu("LoadingMenu");

            if (PhotonNetwork.InRoom)
            {
                MenuManager.Instance.OpenMenu("RoomMenu");
                //s
                if (!AllPlayersExistInRoom())
                {
                    //ActiveReady(PhotonNetwork.LocalPlayer);

                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        //if (IsReady(player))
                            Instantiate(HexafallLauncher.Instance.playerListItemPrefab, HexafallLauncher.Instance.playerListContent.transform).GetComponent<PlayerListItem>().SetUp(player);
                    }
                }

                PhotonNetwork.AutomaticallySyncScene = true;
                GameObject startGameButton = GameObject.Find("StartGameButton");

                startGameButton?.SetActive(PhotonNetwork.IsMasterClient);
            }
        }
    }
    public bool IsReady(Player player)
    {
        bool ret = false;

        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "PlayerListItem(Clone)");

        foreach (GameObject g in objects)
        {
            if (g.GetComponent<PlayerListItem>().player == player)
            {
                if (g.GetComponent<PlayerListItem>().ready)
                {
                    ret = true;
                    break;
                }
            }

        }

        return ret;
    }

    public void ActiveReady(Player player)
    {

        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "PlayerListItem(Clone)");

        foreach (GameObject g in objects)
        {
            if (g.GetComponent<PlayerListItem>().player == player)
            {
                g.GetComponent<PlayerListItem>().ready = true;
                return;

            }

        }

        return;
    }

    public bool AllPlayersExistInRoom()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "PlayerListItem(Clone)");

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            bool ret = false;
            foreach(GameObject g in objects)
            {
                if(g.GetComponent<PlayerListItem>().player == player)
                {
                    ret = true;
                }
                
            }
            if (!ret)
                return ret;
        }

        return true;
    }

    public bool ClientManagerExist()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "ClientManager(Clone)");
        if(objects != null)
            return false;

        return true;
    }
}
