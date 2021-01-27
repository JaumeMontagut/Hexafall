using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class ClientManager : MonoBehaviour
{
    public PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity).GetComponent<PlayerVars>();
    }
}
