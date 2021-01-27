﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerVars : MonoBehaviour
{
   
    [ShowOnly] public HexagonalTile currentPlatform;

    public float surfacePos;

    private PlayerMove playerMove;
    [HideInInspector] public Color emissiveColor;
    [HideInInspector] public Vector3 offset;

    public static Color [] colors = new Color[5] 
                        { 
                          Color.cyan, 
                          Color.green, 
                          Color.magenta , 
                          Color.red,
                          Color.blue,
                         };
    float intersity = 1F;
    public Color color;

    Material[] materials;

    PhotonView photonView;

   // List<int> colorsTaken = new List<int>();
    public bool falling 
    {
        get; private set; 
    }

   public bool moving
    {
        get; private set;
    }

  

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        photonView = GetComponent<PhotonView>();

        Managers.Game.players.Add(gameObject);

        moving = false;
        offset = new Vector3(0.0f, 0.0f, 0.0f);

        #region Change Color
            
        if(photonView.IsMine)
        {
            int index;
            index = Random.Range(0, colors.Length);

            photonView.RPC("ChangePlayerColor", RpcTarget.All, index);
        }
      
        #endregion
    }

    [PunRPC]
    public void ChangePlayerColor(int index)
    {
        //Debug.LogError("Changing colors " + index);
        color = colors[index];

        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        materials = renderer.materials;

        materials[0].SetColor("_EmissionColor", color * intersity);
        materials[1].SetColor("_EmissionColor", color * intersity);
      //  colorsTaken.Add(index);
    }

    public void ActivateFalling()
    {
        playerMove.timeFalling = 0.0f;
        falling = true;

        //currentPlatform.GetComponent<Platform>().ReduceAlpha();
        //currentPlatform.GetComponent<Platform>().StartRestoringAlphaWithFade();
    }

    public void DesactivateFalling()
    {
        playerMove.timeFalling = 0.0f;
        falling = false;

    }

    public void ActiveMoving()
    {
        moving = true;
    }

    public void DesactivateMoving()
    {
        moving = false;
    }

}
