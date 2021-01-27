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

    float timeJustFall = 0;

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
      
    }
    
    private void Start()
    {
        transform.position = Managers.Tiles.start.transform.position + ReturnOffset();
        playerVars.currentPlatform = Managers.Tiles.start;
        Managers.Game.players.Add(gameObject);
    }
    public Vector3 ReturnOffset()
    {
        return new Vector3(Managers.Game.offsets[playerVars.identificator].x, 0.1f, Managers.Game.offsets[playerVars.identificator].y);
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
        Vector3 moveVec = nextPlatform.transform.position + ReturnOffset() - transform.position;
        moveSpeed = (float) moveVec.magnitude / 0.6F* 3;
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
            Vector3 moveVec = nextPlatform.transform.position + ReturnOffset() - transform.position;
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
            //Managers.Audio.PlayAudio("Death");
            playerVars.ActivateFalling();
            nextPlatform.PlayAnimation();
            timeJustFall = Time.time + 0.50F;
            selectPlatform.SelectedPlatform = null;
            playerVars.currentPlatform = Managers.Tiles.start;
        }
        else
        {
            playerVars.currentPlatform = nextPlatform;
     
        }

        if (playerVars.currentPlatform == Managers.Tiles.end)
        {
            //This player wins!!
            EventManager.TriggerEvent(MyEventType.PlayerReachGoal, gameObject/*the player*/);

            if (photonView.IsMine)
            {
                animator.Play("Dance");
            }
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
