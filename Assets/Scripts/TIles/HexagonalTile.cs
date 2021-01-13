using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clockwise Hexagon Directions https://catlikecoding.com/unity/tutorials/hex-map/part-2/cell-neighbors/directions.png
public class HexagonalTile : MonoBehaviour
{
    public Vector2Int gridPosition;
    public HexagonalTile[] neighborTiles;

    private void Awake()
    {
        neighborTiles = new HexagonalTile[6];
    }

    public List<HexagonalTile> GetAdjacentTiles()
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
}