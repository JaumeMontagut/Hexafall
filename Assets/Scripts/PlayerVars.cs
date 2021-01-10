using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerVars : MonoBehaviour
{
    [ShowOnly] public GameObject currentPlatform;
    
    public float surfacePos;
    public bool mainPlayer; // If the player Game object is the current player of this machine.

    private PlayerMove playerMove;

    public bool falling 
    {
        get; private set; 
    }
    [ShowOnly] public bool moved = false;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
