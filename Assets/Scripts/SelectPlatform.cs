﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlatform : MonoBehaviour
{
    public PlayerVars playerVars;
    public PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        playerVars = GetComponent<PlayerVars>();
        playerMove = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //Idea: Need a manager of turns that activate this component when the turns start.
        //      Desactivate this component when select a new Hexagon and move


        if (Input.GetMouseButtonDown(0)) // Maybe this may be GetMouseButtonUp ??
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    //TODO: if a character or something is between the mouse and the hexagon, it wil select that object and not the hexagon, not entering here!
                    //          - posible solution: pick all the game objects that intersect with the ray and compare with all of them.


                    foreach(GameObject adjacentHexagon in playerVars.currentPlatform.GetComponent<Platform>().adjacentPlatforms)
                    {
                        if(adjacentHexagon == hitInfo.transform.gameObject)
                        {
                            print("It's an available platform!!");

                            //Try to move the player to the new hexagon.
                            if (playerMove.Move(hitInfo.transform.position))
                            {
                                print("Player moved succesfully!!");

                                //check if its path and if it's not, active the player falling.
                                if (adjacentHexagon.GetComponent<Platform>().isPath)
                                {
                                    //Update the currentHexagon of the player
                                    playerVars.currentPlatform = adjacentHexagon;

                                }
                                else
                                {
                                    playerVars.ActivateFalling();

                                }


                            }

                            else
                                print("ERROR: Problem when trying to move the player to the new platform!");

                            break;

                        }
                    }


                }
            }
        }
    }
}
