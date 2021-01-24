using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlatform : MonoBehaviour
{
    private PlayerVars playerVars;
    private PlayerMove playerMove;

    void Start()
    {
        playerVars = GetComponent<PlayerVars>();
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        //Idea: Need a manager of turns that activate this component when the turns start.
        //      Desactivate this component when select a new Hexagon and move
        //Temporally to prevent in test to movve if its falling -> this will not be needed when turns manager is implemented.

        if (playerMove.IsMine()
            && !playerVars.falling
            && !playerVars.moving
            && Input.GetMouseButtonDown(0)
            && playerMove.AvailableMovements > 0)
        {
            //TODO: if a character or something is between the mouse and the hexagon, it wil select that object and not the hexagon, not entering here!
            //          - posible solution: pick all the game objects that intersect with the ray and compare with all of them.
            RaycastHit hitInfo = new RaycastHit();


            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 100f, LayerMask.GetMask("Platform")))
            {
                List<HexagonalTile> neighbors = playerVars.currentPlatform.GetNeighbors();

                foreach (HexagonalTile neighbor in neighbors)
                {
                    if (neighbor.gameObject == hitInfo.transform.gameObject)
                    {
                        //Try to move the player to the new hexagon.
                        if (!playerMove.StartMoving(neighbor))
                        {
                            print("ERROR: Problem when trying to move the player to the new platform!");
                        }
                        break;
                    }
                }
            }
        }
    }
}
