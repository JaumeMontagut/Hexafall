using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clockwise Hexagon Directions https://catlikecoding.com/unity/tutorials/hex-map/part-2/cell-neighbors/directions.png
public class HexagonalTile : MonoBehaviourPunCallbacks, IPunObservable
{
    public ElasticMove elasticMove;

    // Grid ------------------

    public Vector2Int gridPosition;
    public HexagonalTile[] neighborTiles;
    public float cost = 0f;

    // Game -------------------

    public bool isPath = false;

    private void Awake()
    {
        neighborTiles = new HexagonalTile[(int)HexagonDirections.MAX];
    }

    private void Start()
    {
        transform.parent = Managers.Tiles.transform;
    }
    public List<HexagonalTile> GetNeighbors()
    {
        List<HexagonalTile> ret = new List<HexagonalTile>();

        for (HexagonDirections direction = 0; direction < HexagonDirections.MAX; ++direction)
        {
            if (neighborTiles[(int)direction] != null)
            {
                ret.Add(neighborTiles[(int)direction]);
            }
        }

        return ret;
    }

    public HexagonalTile GetAdjacentTile(HexagonDirections direction)
    {
        return neighborTiles[(int)direction];
    }

    public void StartStaggerMovement()
    {
        elasticMove.StartMoving();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Neighbors
            for (int i = 0; i < 6; i++)
            {
                int id = (neighborTiles[i]) ? neighborTiles[i].photonView.ViewID : -1;
                stream.SendNext(id);
            }
            stream.SendNext(isPath);
        }
        else
        {
            // Neighbors
            for (int i = 0; i < 6; i++)
            {
                int id = (int)stream.ReceiveNext();
                if (id == -1) continue;
                if (neighborTiles[i] != null && neighborTiles[i].photonView.ViewID == id) continue;
                neighborTiles[i] = PhotonNetwork.GetPhotonView(id).gameObject.GetComponent<HexagonalTile>();
            }
            isPath = (bool)stream.ReceiveNext();
        }
    }
}