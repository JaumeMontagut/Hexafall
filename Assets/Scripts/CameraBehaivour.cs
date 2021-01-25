using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyEvents;


public class CameraBehaivour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Action<dynamic> function = MoveToWinPos;
        EventManager.StartListening(MyEventType.PlayerReachGoal, MoveToWinPos);
    }

    private void OnDisable()
    {
        Action<dynamic> function = MoveToWinPos;
        EventManager.StopListening(MyEventType.PlayerReachGoal, function);
    }

    private void MoveToWinPos(dynamic info)
    {
        //Move Camera

        GameObject endPlatform = Managers.Tiles.end.gameObject;
        Camera.main.transform.position = new Vector3(endPlatform.transform.position.x + 2, endPlatform.transform.position.y + 0.5f, endPlatform.transform.position.z);
        Camera.main.transform.rotation = Quaternion.Euler(0, -90, 0);
    }
}
