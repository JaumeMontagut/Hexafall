using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using System.IO;

// Clockwise Hexagon Directions https://catlikecoding.com/unity/tutorials/hex-map/part-2/cell-neighbors/directions.png
// Cordinate Hexagonal Grid https://media.discordapp.net/attachments/795280554890231861/798741491991707678/unknown.png
public enum HexagonDirections { N = 0, NE = 1, SE = 2, S = 3, SW = 4, NW = 5, MAX = 6 };
public class TilesManager : MonoBehaviour
{
    #region Atributes
    public GameObject tilePrefab;
    public float tileMargin = 0.1f;
    [Range(1, 10)]
    public int magnitude = 1;
    private float tileHeight = 1f, tileWidth = 1f;
    private static Vector2Int[] neighborOffsets = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1) };

    #endregion
    [HideInInspector] public PhotonView photonView;
    private Dictionary<Vector2Int, HexagonalTile> grid;

    public HexagonalTile start { get; private set; }
    public HexagonalTile end { get; private set; }

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        Managers.Tiles = this;
        grid = new Dictionary<Vector2Int, HexagonalTile>();

        // Set Tile Dimmension
        Vector3 tileSize = tilePrefab.GetComponent<Renderer>().bounds.size;
        tileWidth = tileSize.x + tileMargin;
        tileHeight = tileSize.z + tileMargin;
    }

    [PunRPC]
    void SetPath(int startId, int endPos, int[] pathIds)
    {
        start = PhotonNetwork.GetPhotonView(startId).GetComponent<HexagonalTile>();
        end =  PhotonNetwork.GetPhotonView(endPos).GetComponent<HexagonalTile>();

        foreach (int id in pathIds)
        {
            PhotonNetwork.GetPhotonView(id).GetComponent<HexagonalTile>().isPath = true;
        }
    }

    public void OnDrawGizmos()
    {
        //foreach (var item in path)
        //{
        //    Gizmos.color = item.color;
        //    Gizmos.DrawSphere(item.tile.transform.position + new Vector3(0, 0.2f, 0), 0.16f);
        //}
    }

    public void GenerateTiles()
    {
        int topLimit = 0;
        int botLimit = magnitude;

        for (int column = -magnitude; column <= magnitude; ++column)
        {
            for (int row = topLimit; row <= botLimit; ++row)
                InstantiateTile(new Vector2Int(column, row) );
            if (column < 0) --topLimit;
            else            --botLimit;
        }

        foreach(HexagonalTile tile in grid.Values)
        {
            FillNeighborTiles(tile);
        }
    }
    private void InstantiateTile(Vector2Int gridPosition)
    {
        GameObject intance = PhotonNetwork.InstantiateRoomObject(
            Path.Combine("PhotonPrefabs", "HexagonalTile"),
            GridToWorld(gridPosition, tileHeight, tileWidth),
            Quaternion.AngleAxis(60, Vector3.up));

        //GameObject 
        HexagonalTile hexagonalTile = intance.GetComponent<HexagonalTile>();
        hexagonalTile.gridPosition = gridPosition;
        grid.Add(gridPosition, hexagonalTile);
    }
    public void FillNeighborTiles(HexagonalTile tile)
    {
        for (HexagonDirections direction = 0; direction < HexagonDirections.MAX; ++direction)
        {
            Vector2Int neighborGridPosition = tile.gridPosition + neighborOffsets[(int)direction];

            if (grid.ContainsKey(neighborGridPosition))
            {
                tile.neighborTiles[(int)direction] = grid[neighborGridPosition];
            }
        }
    }
    public Vector3 GridToWorld(Vector2Int gridPosition, float tileHeight, float tileWidth)
    {
        int column = gridPosition.x, row = gridPosition.y;
        Vector3 worldPosition = new Vector3(0f, 0f, 0f);
        worldPosition.z += tileHeight * (float)row;
        worldPosition.z += tileHeight * 0.5f * column;
        worldPosition.x += ( (tileWidth * 3f ) / 4f ) * column;
        return worldPosition;
    }
    public HexagonalTile GetTileAtPosition(Vector2Int position)
    {
        return grid[position];
    }
    public HexagonalTile GetRandomTile()
    {
        return grid.ElementAt(Random.Range(0, grid.Count)).Value;
    }
    public void ClearTilesCost()
    {
        foreach(HexagonalTile tile in grid.Values)
        {
            tile.cost = 0f;
        }
    }
}
