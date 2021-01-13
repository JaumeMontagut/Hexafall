using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] Text text;
    public RoomInfo info;

    public void SetUp(RoomInfo info)
    {
        text.text = info.Name;
        this.info = info;
    }

    public void OnClick()
    {
        HexafallLauncher.Instance.JoinRoom(info);
    }
}
