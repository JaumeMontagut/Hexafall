using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyEvents;




public class CameraBehaivour : MonoBehaviour
{
    public float timeInTransition;
    [ShowOnly] public float transitionTimer;

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
        Vector3 finalPos = new Vector3(endPlatform.transform.position.x + 2, endPlatform.transform.position.y + 0.5f, endPlatform.transform.position.z);
        Quaternion finalRot = Quaternion.Euler(0, -90, 0);

        StartCoroutine(TransitionCamera(transform.position, finalPos, transform.rotation, finalRot));
    }

    IEnumerator TransitionCamera(Vector3 initialPos, Vector3 finalPos, Quaternion initialRot, Quaternion finalRot)
    {
        while(transitionTimer < timeInTransition)
        {
            float ratio = transitionTimer / timeInTransition;

            transform.position = Vector3.Lerp(initialPos, finalPos, ratio);
            transform.rotation = Quaternion.Lerp(initialRot, finalRot,ratio);

            transitionTimer += Time.deltaTime;
            yield return null;
        }
        
    }
}
