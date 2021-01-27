using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyEvents;




public class CameraBehaivour : MonoBehaviour
{
    public float timeInTransition;
    [ShowOnly] public float transitionTimer;
    private bool thisFrame = true;

    void Update()
    {
        thisFrame = true;
    }

    private void OnEnable()
    {
        EventManager.StartListening(MyEventType.PlayerReachGoal, MoveToWinPos);
    }
    private void OnDisable()
    {
        EventManager.StopListening(MyEventType.PlayerReachGoal, MoveToWinPos);
    }
    private void MoveToWinPos(object info)
    {
        //Move Camera

        GameObject endPlatform = Managers.Tiles.end.gameObject;
        Vector3 finalPos = new Vector3(endPlatform.transform.position.x + 2, endPlatform.transform.position.y + 0.5f, endPlatform.transform.position.z);
        Quaternion finalRot = Quaternion.Euler(0, -90, 0);
        StartCoroutine(TransitionCamera(transform.position, finalPos, transform.rotation, finalRot));
        foreach (GameObject player in Managers.Game.players)
        {
            PlayerMove movePlayer = player.GetComponent<PlayerMove>();
            movePlayer.enableInput = false;
            movePlayer.selectPlatform.SelectedPlatform = null;

        }
    }

    IEnumerator TransitionCamera(Vector3 initialPos, Vector3 finalPos, Quaternion initialRot, Quaternion finalRot)
    {
        //Debug.Log("new change");

        Debug.Log("new change");
        while(transitionTimer < timeInTransition)
        {
            if (thisFrame)
            {
                float ratio = transitionTimer / timeInTransition;

                transform.position = Vector3.Lerp(initialPos, finalPos, ratio);
                transform.rotation = Quaternion.Lerp(initialRot, finalRot, ratio);

                transitionTimer += Time.deltaTime;
                Debug.Log(Time.deltaTime);
                Debug.Log(transitionTimer);
            }
            thisFrame = false;

            yield return null;


        }

    }
}
