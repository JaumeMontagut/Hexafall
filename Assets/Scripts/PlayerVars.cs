using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerVars : MonoBehaviour
{
    public GameObject startingPlatform;

    [ShowOnlyAttribute]
    public GameObject currentPlatform;
    
    public float surfacePos;

    [HideInInspector]
    public PlayerMove playerMove;

    public bool mainPlayer; // If the player Game object is the current player of this machine.
    public bool falling 
    {
        get; private set; 
    }
    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        currentPlatform = startingPlatform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateFalling()
    {
        playerMove.timeFalling = 0.0f;
        falling = true;
    }

    public void DesactivateFalling()
    {
        
        falling = false;
    }

}
