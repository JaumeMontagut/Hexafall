using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator
{
    static float costIncrement = 100f;
    static int numOfPath = 3;

    public static List<int> GeneratePath()
    {
        List<int>  pathIds = new List<int>();
        HexagonalTile start = Managers.Tiles.GetTileAtPosition(Managers.Game.startPosition);
        pathIds.Add(start.GetPhotonID()); // Add Start

        List<HexagonalTile> pathTiles = new List<HexagonalTile>();
        AddRandomPaths(pathTiles, start, numOfPath);

        foreach (HexagonalTile tile in pathTiles)
        {
            pathIds.Add(tile.GetPhotonID());
        }

        Managers.Tiles.ClearTilesCost();

        return pathIds;
    }

    private static void AddRandomPaths( List<HexagonalTile> path, HexagonalTile start, int numPaths)
    {
        HexagonalTile currentTile = start;

        for (int i = 0; i < numPaths; i++)
        {
            currentTile = AddRandomPath(path ,currentTile);
        }
    }

    private static HexagonalTile AddRandomPath(List<HexagonalTile> path, HexagonalTile startPath)
    {
        AddCostAround(startPath);
        HexagonalTile randomTile;
        do
        {
            randomTile = Managers.Tiles.GetRandomTile();
        }
        while (randomTile.cost != 0f);

        path.AddRange(Pathfinding.GeneratePath(startPath, randomTile));

        return randomTile;
    }

    private static void AddCostAround(HexagonalTile toAddCost)
    {
        toAddCost.cost += costIncrement * 2f;
        List<HexagonalTile> neighbors = toAddCost.GetNeighbors();
        foreach (HexagonalTile tile in neighbors)
        {
            tile.cost += costIncrement;
        }
    }


}
