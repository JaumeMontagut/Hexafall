﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class PlayerMove : MonoBehaviour
{
    public float timeToFall = 1.0f;
    public float fallDistance = 2.0f;

    [ShowOnly] public float timeFalling = 0.0f;

    private PlayerVars playerVars;
    private Animator animator;
    private HexagonalTile nextPlatform;
    float jumpStart = 0;
    float moveSpeed = 2f;
    PhotonView photonView;

    int availableMovements = 1;

    const float onIntensity = 21f;
    const float offIntensity = 7f;

    public int AvailableMovements
    {
        get
        {
            return availableMovements;
        }
        set
        {
            availableMovements = value;
            if (availableMovements > 0)
            {
                foreach (Material material in GetComponentInChildren<SkinnedMeshRenderer>().materials)
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetVector("_EmissionColor", playerVars.emissiveColor * onIntensity);
                }
            }
            else
            {
                foreach (Material material in GetComponentInChildren<SkinnedMeshRenderer>().materials)
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetVector("_EmissionColor", playerVars.emissiveColor * offIntensity);
                }
            }
        }
    }


    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerVars = GetComponent<PlayerVars>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //Set the starting emission color on playerVars
        Material[] playerMaterials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        playerVars.emissiveColor = playerMaterials[0].GetColor("_EmissionColor");
        foreach (Material mat in playerMaterials)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetVector("_EmissionColor", playerVars.emissiveColor * onIntensity);
        }

        transform.position = Managers.Tiles.start.transform.position;
        playerVars.currentPlatform = Managers.Tiles.start;

        Managers.Game.players.Add(gameObject);
    }

    public bool IsMine()
    {
        return Managers.Game.playLocal || photonView.IsMine;
    }

    void Update()
    {
        if (IsMine())
        {
            if (playerVars.falling)
            {
                Fall();
            }
            if (playerVars.moving)
            {
                Moving();
            }
        }
    }

    public bool StartMoving(HexagonalTile platform)
    {
        AvailableMovements--;
        animator.SetTrigger("Jump");
        nextPlatform = platform;
        playerVars.ActiveMoving();

        // Set rotation
        Vector3 moveVec = nextPlatform.transform.position - transform.position;
        float angle = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, angle, 0);
        jumpStart = Time.time;
        

        return true;
    }
    bool doElasticAnimation = true;
    public void Moving()
    {
        
        Vector3 moveVec = (nextPlatform.transform.position + playerVars.offset) - transform.position;
        if(Time.time - jumpStart >= 0.35F)
        {
            transform.position += new Vector3( moveVec.normalized.x, 0 , moveVec.normalized.z ) * Time.deltaTime * moveSpeed;

            if (moveVec.sqrMagnitude <= 0.05 && doElasticAnimation)
            {
                nextPlatform.GetComponent<ElasticMove>().StartMoving();
                doElasticAnimation = false;
            }
            if (moveVec.sqrMagnitude <= 0)
            {
                EndMove();
            }
            
        }
        
    }
    public bool EndMove()
    {
        ///Move the player to the new hexagon.

        bool ret = false;
        doElasticAnimation = true;
        //transform.position = nextPlatform.transform.position;
        playerVars.DesactivateMoving();

        //Update the currentHexagon of the player
        playerVars.currentPlatform = nextPlatform;
        
        //check if its path and if it's not, active the player falling.
        if (!nextPlatform.isPath)
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
        transform.position = Managers.Tiles.start.transform.position + playerVars.offset;
        playerVars.currentPlatform = Managers.Tiles.start;
    }
}
