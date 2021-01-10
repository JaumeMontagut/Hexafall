using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

public class MapManager : MonoBehaviour
{
    public GameObject[] allPlatforms;
    public float starAltitude = 0.0f;

    [ShowOnly] public GameObject startingPlatform;
    [ShowOnly] public GameObject endingPlatform;
    [ShowOnly] public GameObject star;


    private GameObject player;
    private PlayerVars playerVars;

    public
    // Start is called before the first frame update
    void Start()
    {

        SearchStartingAndEndingPlatforms();

        SetPlayersToStart();
        
        SetStar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    private void SearchStartingAndEndingPlatforms()
    {
        ///All of the next section of the code it's only for check if everything is alright and EXIT the game if it's not.
        ///And it's nedded to find the starting and ending platform!


        int startPlatforms = 0;
        int endPlatforms = 0;

        //Check how much starting platforms and endign platforms are in the game setted.
        foreach (GameObject currentPlatform in allPlatforms)
        {
            if (currentPlatform.GetComponent<Platform>().isStart)
            {
                ++startPlatforms;
                startingPlatform = currentPlatform;
            }

            if (currentPlatform.GetComponent<Platform>().isEnd)
            {
                ++endPlatforms;
                endingPlatform = currentPlatform;
            }
        }

        //Check if it's the correct number of starting platforms.
        if (startPlatforms != 1)
        {
            if (startPlatforms < 1)
                Debug.LogError("There is no start platform setted!!");

            else
                Debug.LogError("There is more than 1 start platform setted!!");

            //if there isn't the correct numbers of starting platform, EXIT the game. This is a case that we don't want to occur and continious the game!
            EditorApplication.isPlaying = false;

        }

        //Check if it's the correct number of ending platforms.
        if (endPlatforms != 1)
        {
            if (endPlatforms < 1)
                Debug.LogError("There is no end platform setted!!");

            else
                Debug.LogError("There is more than 1 end platform setted!!");

            //if there isn't the correct numbers of ending platform, EXIT the game. This is a case that we don't want to occur and continious the game!
            EditorApplication.isPlaying = false;
            return;

        }
    }

    

    private void SetPlayersToStart()
    {
        ///Set the player (players when photon imlemented) to the starting platform


        //Search by tag because it will be more players and we want all. This wil need to change te FindGameObjectWithTag to FindGameObjectsWithTag (in plural) and change the var to an array.
        //TODO: do it for all players when Photon is implemented.
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Can't find the gameobject 'Player', check there is a player with the TAG 'Player' to be able to find that gameobject!");

            //Exit the game if can't find the player... Althought, it will crash without it. -shrug-
            EditorApplication.isPlaying = false;
            return;

        }

        playerVars = player.GetComponent<PlayerVars>();

        //move the player to the starting platform.
        player.transform.position = new Vector3(startingPlatform.transform.position.x, playerVars.surfacePos, startingPlatform.transform.position.z);

        //Set the starting platform to the current platform for the player.
        playerVars.currentPlatform = startingPlatform;
    }


    private void SetStar()
    {
        ///Set the star position


        star = GameObject.Find("Star");
        if (star == null)
        {
            Debug.LogError("Can't find the gameobject 'Star', check there is a gameObject with the NAME 'Star' to be able to find that gameobject!");

            //Exit the game if can't find the player... Althought, it will crash without it. -shrug-
            EditorApplication.isPlaying = false;
            return;

        }

        //Move the star to the ending platform.
        star.transform.position = new Vector3(endingPlatform.transform.position.x, endingPlatform.transform.position.y + starAltitude, endingPlatform.transform.position.z);
    }


}
