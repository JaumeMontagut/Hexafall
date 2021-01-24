using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

public class MapManager : MonoBehaviour
{
    public GameObject rootPlatforms;
    public float starAltitude = 0.0f;

    [ShowOnly] public GameObject startingPlatform;
    [ShowOnly] public GameObject endingPlatform;
    [ShowOnly] public GameObject star;

    private void Awake()
    {
        //SearchStartingAndEndingPlatforms();
        //SetStar();
    }

    //public void SetPlayerToStartPlatform(GameObject player)
    //{
    //    PlayerVars playerVars = player.GetComponent<PlayerVars>();

    //    player.transform.position = new Vector3(startingPlatform.transform.position.x, playerVars.surfacePos, startingPlatform.transform.position.z);
    //    playerVars.currentPlatform = startingPlatform;
    //}

//    private void SearchStartingAndEndingPlatforms()
//    {
//        ///All of the next section of the code it's only for check if everything is alright and EXIT the game if it's not.
//        ///And it's nedded to find the starting and ending platform!


//        int startPlatforms = 0;
//        int endPlatforms = 0;

//        //Check how much starting platforms and endign platforms are in the game setted.
//        foreach (Transform currentPlatformTransform in rootPlatforms.transform)
//        {
//            GameObject currentPlatform = currentPlatformTransform.gameObject;
//            if(currentPlatform.GetComponent<Platform>() == null)
//            {
//                Debug.LogError("GameObjacet name " + currentPlatform.name);
//            }
//            if (currentPlatform.GetComponent<Platform>().isStart)
//            {
//                ++startPlatforms;
//                startingPlatform = currentPlatform;
//            }

//            if (currentPlatform.GetComponent<Platform>().isEnd)
//            {
//                ++endPlatforms;
//                endingPlatform = currentPlatform;
//            }
//        }

//        //Check if it's the correct number of starting platforms.
//        if (startPlatforms != 1)
//        {
//            if (startPlatforms < 1)
//                Debug.LogError("There is no start platform setted!!");

//            else
//                Debug.LogError("There is more than 1 start platform setted!!");

//#if UNITY_EDITOR
//            //if there isn't the correct numbers of starting platform, EXIT the game. This is a case that we don't want to occur and continious the game!
//            EditorApplication.isPlaying = false;
//#endif

//        }

//        //Check if it's the correct number of ending platforms.
//        if (endPlatforms != 1)
//        {
//            if (endPlatforms < 1)
//                Debug.LogError("There is no end platform setted!!");

//            else
//                Debug.LogError("There is more than 1 end platform setted!!");

//            //if there isn't the correct numbers of ending platform, EXIT the game. This is a case that we don't want to occur and continious the game!
//#if UNITY_EDITOR
//            EditorApplication.isPlaying = false;
//#endif
//            return;

//        }
//    }


//    private void SetStar()
//    {
//        ///Set the star position


//        star = GameObject.Find("Star");
//        if (star == null)
//        {
//            Debug.LogError("Can't find the gameobject 'Star', check there is a gameObject with the NAME 'Star' to be able to find that gameobject!");

//            //Exit the game if can't find the player... Althought, it will crash without it. -shrug-
//#if UNITY_EDITOR
//            EditorApplication.isPlaying = false;
//#endif
//            return;

//        }

//        //Move the star to the ending platform.
//        star.transform.position = new Vector3(endingPlatform.transform.position.x, endingPlatform.transform.position.y + starAltitude, endingPlatform.transform.position.z);
//    }


}
