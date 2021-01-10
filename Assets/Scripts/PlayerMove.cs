using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float timeToFall = 1.0f;
    public float fallDistance = 2.0f;

    [ShowOnly]
    public float timeFalling = 0.0f;

    private PlayerVars playerVars;
    // Start is called before the first frame update
    void Start()
    {
        playerVars = GetComponent<PlayerVars>();
    }

    // Update is called once per frame
    void Update()
    {
        timeFalling += Time.deltaTime;

        if (playerVars.falling)
        {
            Fall();
        }
    }

    public bool Move(Vector3 destination)
    {
        //Move the player to the new hexagon.

        bool ret = false;

        transform.position = new Vector3(destination.x, transform.position.y, destination.z);
        ret = true;


        return ret;
    }

    public void Fall()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (fallDistance * Time.deltaTime), transform.position.z);

        GameObject wrongPlatform = playerVars.currentPlatform;

        ChangeAlpha(wrongPlatform, wrongPlatform.GetComponent<Platform>().alphaValue);


        if (timeFalling >= timeToFall)
        {
            Respawn();
            playerVars.DesactivateFalling();
        }
    }

    public void Respawn()
    {
        GameObject wrongPlatform = playerVars.currentPlatform;

        ChangeAlpha(wrongPlatform, 1.0f);

        transform.position = new Vector3(playerVars.startingPlatform.transform.position.x, playerVars.surfacePos, playerVars.startingPlatform.transform.position.z);
        playerVars.currentPlatform = playerVars.startingPlatform;
    }

    private void ChangeAlpha(GameObject gameObject, float alpha)
    {
        Color currentColor = gameObject.GetComponent<Renderer>().material.color;
        currentColor.a = alpha;
        gameObject.GetComponent<Renderer>().material.color = currentColor;
    }
}
