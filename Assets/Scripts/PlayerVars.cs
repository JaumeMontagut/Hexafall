using System.Collections;
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

    public static Color [] colors = {
                          Color.green, 
                          Color.magenta , 
                          Color.red,
                          Color.blue
                         };
    float intensity = 5F;
    public Color color;

    Material[] materials;

    PhotonView photonView;
    public int identificator = -1;

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

        PlayerVars[] players = FindObjectsOfType<PlayerVars>();


        //Debug.Log(photonView.CreatorActorNr);
        //Debug.Log(PhotonNetwork.PlayerList[identificator].ActorNumber);

        for(int i = 0; i < 4; ++i)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == photonView.CreatorActorNr)
            {
                identificator = i;
                break;
            }
        }

        ChangePlayerColor(identificator);

    }

    [PunRPC]
    public void ChangePlayerColor(int index)
    {
        //Debug.LogError("Changing colors " + index);
        color = colors[index];

        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        materials = renderer.materials;

        materials[0].EnableKeyword("_EMISSION");
        materials[0].SetColor("_EmissionColor", color * intensity);
        materials[0].EnableKeyword("_EMISSION");
        materials[1].SetColor("_EmissionColor", color * intensity);
      //  colorsTaken.Add(index);
    }

    public void ActivateFalling()
    {
        playerMove.timeFalling = 0.0f;
        falling = true;
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
