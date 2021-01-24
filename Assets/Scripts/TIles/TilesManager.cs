using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

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
    private float tileHeight = 0f, tileWidth = 0f;
    private static Vector2Int[] neighborOffsets = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1) };

    #endregion
    private PhotonView photonView;
    private Dictionary<Vector2Int, HexagonalTile> grid;

    [HideInInspector] public HexagonalTile start { get; private set; }
    [HideInInspector] public HexagonalTile end { get; private set; }

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        Managers.Tiles = this;
        grid = new Dictionary<Vector2Int, HexagonalTile>();
    }

    void Start()
    {
        // Set Tile Dimmension
        Vector3 tileSize = tilePrefab.GetComponent<Renderer>().bounds.size;
        tileWidth = tileSize.x + tileMargin; 
        tileHeight = tileSize.z + tileMargin;
        GenerateTiles();

        start = grid[Vector2Int.zero];

        Color[] colors = { Color.red, Color.yellow, Color.blue, Color.green, Color.white, Color.cyan };
        AddRandomPaths(5, colors);

        foreach (infos info in path)
        {
            info.tile.isPath = true;
        }

        end = path[path.Count - 1].tile;


    }

    #region HardcodedTest

    struct infos
    {
        public Color color;
        public HexagonalTile tile;
    }

    List<infos> path = new List<infos>();
    float costIncrement = 4f;

    public void AddRandomPaths(int numPaths,  Color[] colors)
    {
        HexagonalTile currentTile = start;

        for (int i = 1; i < numPaths; i++)
        {
            currentTile = AddRandomPath(currentTile, colors[i]);
        }
    }

    public HexagonalTile AddRandomPath(HexagonalTile last, Color color)
    {
        HexagonalTile randomTile;

        do
        {
            randomTile = grid.ElementAt(Random.Range(0, grid.Count)).Value;
        }
        while (randomTile.cost != 0f);

        List<HexagonalTile> list = (Pathfinding.GeneratePath(last, randomTile));

        foreach (HexagonalTile item in list)
        {
            List<HexagonalTile> neighbors = item.GetNeighbors();
            item.cost += costIncrement * 2f;
            foreach (HexagonalTile tile in neighbors)
            {
                tile.cost += costIncrement;
            }

            infos i;
            i.color = color;
            i.tile = item;
            path.Add(i);
        }

        return randomTile;
    }

    public void OnDrawGizmos()
    {
        foreach (var item in path)
        {
            Gizmos.color = item.color;
            Gizmos.DrawSphere(item.tile.transform.position + new Vector3(0, 1f, 0), 0.3f);
        }
    }
    #endregion

    private void GenerateTiles()
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
        GameObject intance = Instantiate(tilePrefab, GridToWorld(gridPosition), Quaternion.AngleAxis(60, Vector3.up), transform);
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
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        int column = gridPosition.x, row = gridPosition.y;
        Vector3 worldPosition = new Vector3(0f, 0f, 0f);
        worldPosition.z += tileHeight * (float)row;
        worldPosition.z += tileHeight * 0.5f * column;
        worldPosition.x += ( (tileWidth * 3f ) / 4f ) * column;
        return worldPosition;
    }
}
