using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVars : MonoBehaviour
{
    [ShowOnly] public GameObject currentPlatform;

    public float surfacePos;

    private PlayerMove playerMove;

    public bool falling 
    {
        get; private set; 
    }

   public bool moving
    {
        get; private set;
    }

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        moving = false;
    }

    public void ActivateFalling()
    {
        playerMove.timeFalling = 0.0f;
        falling = true;

        currentPlatform.GetComponent<Platform>().ReduceAlpha();
        currentPlatform.GetComponent<Platform>().StartRestoringAlphaWithFade();
    }

    public void DesactivateFalling()
    {
        playerMove.timeFalling = 0.0f;
        falling = false;
        
    }

    public void ActiveMoving()
    {
        moving = true;
    }

    public void DesactivateMoving()
    {
        moving = false;
    }

}
