using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHexagon : MonoBehaviour
{
    public PlayerVars playerComponent;
    public PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        playerComponent = GetComponent<PlayerVars>();
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
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Hexagon"))
                {
                    //TODO: if a character or something is between the mouse and the hexagon, it wil select that object and not the hexagon, not entering here!
                    //          - posible solution: pick all the game objects that intersect with the ray and compare with all of them.


                    foreach(Hexagon adjacentHexagon in playerComponent.currentHexagon.adjacentHexagons)
                    {
                        if(adjacentHexagon.gameObject == hitInfo.transform.gameObject)
                        {
                            print("It's an available hexagon!!");

                            //Try to move the player to the new hexagon.
                            if (playerMove.Move(hitInfo.transform.position))
                            {
                                print("Player moved succesfully!!");

                                //Update the currentHexagon of the player
                                playerComponent.currentHexagon = adjacentHexagon;
                            }

                            else
                                print("ERROR: Problem when trying to move the player to the new hexagon!");

                            break;

                        }
                    }


                }
            }
        }


        /*if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Construction")
                {
                    Debug.Log("It's working!");
                }
                else
                {
                    Debug.Log("nopz");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
            Debug.Log("Mouse is down");
        }*/
    }
}
