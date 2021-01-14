using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMove : MonoBehaviour
{
    public float timeToFall = 1.0f;
    public float fallDistance = 2.0f;

    [ShowOnly] public float timeFalling = 0.0f;

    private PlayerVars playerVars;
    private MapManager mapManager;
    private Animator animator;
    private Vector3 destination;
    private GameObject nextPlatform;

    // Start is called before the first frame update
    void Start()
    {
        playerVars = GetComponent<PlayerVars>();
        animator = GetComponent<Animator>();

        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        if (mapManager == null)
        {
            Debug.LogError("Can't find the gameobject 'MapManager', check there is a gameObject with the NAME 'MapManager' to be able to find that gameobject!");

            //Exit the game if can't find the MapManager... Althought, it will crash without it. -shrug-
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            return;

        }
    }

        // Update is called once per frame
        void Update()
    {

        if (playerVars.falling)
        {
            Fall();
        }
        if(playerVars.moving)
        {
            Moving();
        }
    }
    public bool StartMoving(GameObject platform)
    {
        animator.SetTrigger("Jump");
        nextPlatform = platform;
        playerVars.ActiveMoving();
        return true;
    }
    public void Moving()
    {
        
        Vector3 moveVec = nextPlatform.transform.position - transform.position;
        transform.position += moveVec.normalized * Time.deltaTime;
        if(moveVec.sqrMagnitude <= 0.01)
        {
            EndMove();
        }
       
    }
    public bool EndMove()
    {
        ///Move the player to the new hexagon.

        bool ret = false;

        transform.position = nextPlatform.transform.position;
        playerVars.DesactivateMoving();

        //Update the currentHexagon of the player
        playerVars.currentPlatform = nextPlatform;

        //check if its path and if it's not, active the player falling.
        if (!nextPlatform.GetComponent<Platform>().isPath)
        {
            playerVars.ActivateFalling();
        }

        ret = true;

       
        return ret;
    }

    public void Fall()
    {
        timeFalling += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y - (fallDistance * Time.deltaTime), transform.position.z);


        if (timeFalling >= timeToFall)
        {
            Respawn();
            playerVars.DesactivateFalling();
        }
    }

    public void Respawn()
    {
        //Move to the starting platform and assign it as the current platform.
        GameObject startingPlatform = mapManager.startingPlatform;
        transform.position = new Vector3(startingPlatform.transform.position.x, playerVars.surfacePos, startingPlatform.transform.position.z);
        playerVars.currentPlatform = startingPlatform;
    }

    


}
