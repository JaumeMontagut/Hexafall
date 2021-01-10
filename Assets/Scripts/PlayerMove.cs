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

    // Start is called before the first frame update
    void Start()
    {
        playerVars = GetComponent<PlayerVars>();

        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        if (mapManager == null)
        {
            Debug.LogError("Can't find the gameobject 'MapManager', check there is a gameObject with the NAME 'MapManager' to be able to find that gameobject!");

            //Exit the game if can't find the MapManager... Althought, it will crash without it. -shrug-
            EditorApplication.isPlaying = false;
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
    }

    public bool Move(Vector3 destination)
    {
        ///Move the player to the new hexagon.

        bool ret = false;

        transform.position = new Vector3(destination.x, transform.position.y, destination.z);
        playerVars.moved = true;
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
