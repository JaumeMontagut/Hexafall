using System.Collections;
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
    private Vector3 destination;
    private GameObject nextPlatform;
    float jumpStart = 0;

    private PhotonView photonView;
    [SerializeField] private bool debug = false;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        playerVars = GetComponent<PlayerVars>();
        animator = GetComponent<Animator>();

        Managers.Game.players.Add(gameObject);
        Managers.Map.SetPlayerToStartPlatform(gameObject);
    }

    public bool IsMine()
    {
        return debug || photonView.IsMine;
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

    public bool StartMoving(GameObject platform)
    {
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
        
        Vector3 moveVec = nextPlatform.transform.position - transform.position;
        if(Time.time - jumpStart >= 0.35F)
        {
            transform.position += new Vector3( moveVec.normalized.x, 0 , moveVec.normalized.z ) * Time.deltaTime * 1f;

            if (moveVec.sqrMagnitude <= 0.05 && doElasticAnimation)
            {
                nextPlatform.GetComponent<ElasticMove>().StartMoving();
                doElasticAnimation = false;
            }
            if (moveVec.sqrMagnitude <= 0.03)
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
        if (!nextPlatform.GetComponent<Platform>().isPath)
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
        transform.position = new Vector3(Managers.Map.startingPlatform.transform.position.x, playerVars.surfacePos, Managers.Map.startingPlatform.transform.position.z);
        playerVars.currentPlatform = Managers.Map.startingPlatform;
    }
}
