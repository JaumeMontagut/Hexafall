using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using MyEvents;

public class PlayerMove : MonoBehaviour
{
    public bool enableInput = true;
    float timeToFall = 0.2f;
    public float fallDistance = 2.0f;
    [HideInInspector] public SelectPlatform selectPlatform;

    [ShowOnly] public float timeFalling = 0.0f;

    private PlayerVars playerVars;
    
    private Animator animator;
    private HexagonalTile nextPlatform;
    float moveSpeed = 2f;
    PhotonView photonView;

    int availableMovements = 1;

    const float onIntensity = 21f;
    const float offIntensity = 7f;
    float timeJustFall  =0;

    Camera cam;
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

    private void OnEnable()
    {
        EventManager.StartListening(MyEventType.PlayerJumpTop, SetToMove);
        EventManager.StartListening(MyEventType.PlayerEndJump, EndMove);
    }

    private void OnDisable()
    {
        EventManager.StopListening(MyEventType.PlayerJumpTop, SetToMove);
        EventManager.StopListening(MyEventType.PlayerEndJump, EndMove);
    }

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerVars = GetComponent<PlayerVars>();
        selectPlatform = GetComponent<SelectPlatform>();
        animator = GetComponent<Animator>();
        cam = FindObjectOfType<Camera>();
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

        transform.position = Managers.Tiles.start.transform.position + ReturnOffset();
        playerVars.currentPlatform = Managers.Tiles.start;
        Managers.Game.players.Add(gameObject);
    }
    public Vector3 ReturnOffset()
    {
        return new Vector3(Managers.Game.offsets[playerVars.identificator].x, 0, Managers.Game.offsets[playerVars.identificator].y);
    }

    void Update()
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

    [PunRPC]
    public void StartMoving(int platformID)
    {
        if(timeJustFall > Time.time)
        {
            return;
        }
        PhotonView photonTile = PhotonView.Find(platformID);

        //TODO: Get the closest one to the mouse
        animator.SetTrigger("Jump");
        nextPlatform = photonTile.gameObject.GetComponent<HexagonalTile>();
        playerVars.ActiveMoving();

        // Set rotation
        Vector3 moveVec = nextPlatform.transform.position - transform.position;
        moveSpeed = (float) moveVec.magnitude / 0.6F* 2;
        float angle = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, angle, 0);
        timeJustFall = Time.time;
        return;

    }

    
    bool doElasticAnimation = true;
    bool move = false;
    public void Moving()
    {
        if(move)
        {
            Vector3 moveVec = (nextPlatform.transform.position + playerVars.offset) - transform.position + ReturnOffset();
            transform.position += new Vector3(moveVec.normalized.x, 0, moveVec.normalized.z) * Time.deltaTime * moveSpeed;

            if (moveVec.sqrMagnitude <= 0.05 && doElasticAnimation)
            {
                nextPlatform.GetComponent<ElasticMove>().StartMoving();
                doElasticAnimation = false;
            }
        }
    }

    public void SetToMove(object info)
    {
        if ((GameObject)info == gameObject)
        {
            move = true;
        }
    }

    public void EndMove(object info)
    {
        GameObject player = (GameObject)info;
        if (player != gameObject)

            return;
        ///Move the player to the new hexagon.
        transform.position = nextPlatform.transform.position + ReturnOffset();
        move = false;
        doElasticAnimation = true;

        //Update the currentHexagon of the player
        playerVars.DesactivateMoving();

      
        //check if its path and if it's not, active the player falling.
        if (!nextPlatform.isPath)
        {
            playerVars.ActivateFalling();
            nextPlatform.PlayAnimation();
            timeJustFall = Time.time + 0.50F;
            selectPlatform.SelectedPlatform = null;
            playerVars.currentPlatform = Managers.Tiles.start;
        }
        else
        {
            playerVars.currentPlatform = nextPlatform;
            selectPlatform.SelectedPlatform = null;
            Vector3 selectVec = Input.mousePosition - cam.WorldToScreenPoint(playerVars.currentPlatform.transform.position);
            double angle = Mathf.Rad2Deg * Math.Atan2(selectVec.y, selectVec.x);
            HexagonalTile tile =  null;
            if (angle < 30 || angle > 240)
            {
                tile = playerVars.currentPlatform.GetNeighbor(HexagonDirections.N);
            }
            else if(angle < 90)
            {
                tile = playerVars.currentPlatform.GetNeighbor(HexagonDirections.NE);
            }
            else if (angle < 120)
            {
                tile = playerVars.currentPlatform.GetNeighbor(HexagonDirections.SW);
            }
            else if (angle < 150)
            {
                tile = playerVars.currentPlatform.GetNeighbor(HexagonDirections.S);
            }
            else if (angle < 180)
            {
                tile = playerVars.currentPlatform.GetNeighbor(HexagonDirections.SE);
            }
            else if (angle < 210)
            {
                tile = playerVars.currentPlatform.GetNeighbor(HexagonDirections.NW);
            }

            if (tile != null)
            {
                selectPlatform.SelectedPlatform = tile.gameObject;
            }


        }

        if (playerVars.currentPlatform == Managers.Tiles.end)
        {
            //This player wins!!
            EventManager.TriggerEvent(MyEventType.PlayerReachGoal, gameObject/*the player*/);

        }

        return ;
    }

    public void Fall()
    {
        timeFalling += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y - (fallDistance * Time.deltaTime), transform.position.z);

        if (timeFalling >= timeToFall)
        {
            Respawn();
            playerVars.DesactivateFalling();
            //animator.SetTrigger("FallToPlatform");
        }
    }

    public void Respawn()
    {
        //Move to the starting platform and assign it as the current platform.
        transform.position = Managers.Tiles.start.transform.position + playerVars.offset + ReturnOffset();
        playerVars.currentPlatform = Managers.Tiles.start;
    }
}
