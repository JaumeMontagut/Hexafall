using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVars : MonoBehaviour
{
    [ShowOnly] public HexagonalTile currentPlatform;

    public float surfacePos;

    private PlayerMove playerMove;
    [HideInInspector] public Color emissiveColor;
    [HideInInspector] public Vector3 offset;
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
        offset = new Vector3(0.0f, 0, 0.0f);
    }

    public void ActivateFalling()
    {
        playerMove.timeFalling = 0.0f;
        falling = true;

        //currentPlatform.GetComponent<Platform>().ReduceAlpha();
        //currentPlatform.GetComponent<Platform>().StartRestoringAlphaWithFade();
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
