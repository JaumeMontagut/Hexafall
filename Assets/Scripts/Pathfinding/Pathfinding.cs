using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Pathfinding
{
    // Conduct the A* search
    public static List<HexagonalTile> GeneratePath(HexagonalTile origin, HexagonalTile destiny)
    {
        #region Generate Path

        Dictionary<HexagonalTile, HexagonalTile> cameFrom = new Dictionary<HexagonalTile, HexagonalTile>();
        Dictionary<HexagonalTile, float> costSoFar = new Dictionary<HexagonalTile, float>();

        var frontier = new PriorityQueue<HexagonalTile>();
        // Add the starting location to the frontier with a priority of 0
        frontier.Enqueue(origin, 0f);

        cameFrom.Add(origin, origin); 
        costSoFar.Add(origin, 0f);

        while (frontier.Count > 0f)
        {
            // Get the Location from the frontier that has the lowest
            // priority, then remove that Location from the frontier
            HexagonalTile current = frontier.Dequeue();

            // If we're at the goal Location, stop looking.
            if (current.Equals(destiny)) break;

            // Neighbors will return a List of valid tile Locations
            // that are next to, diagonal to, above or below current

            List<HexagonalTile> neighbors = current.GetNeighbors();


            foreach (var neighbor in neighbors)
            {
                float newCost = costSoFar[current] + neighbor.cost;

                // If there's no cost assigned to the neighbor yet, or if the new
                // cost is lower than the assigned one, add newCost for this neighbor
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    // If we're replacing the previous cost, remove it
                    if (costSoFar.ContainsKey(neighbor))
                    {
                        costSoFar.Remove(neighbor);
                        cameFrom.Remove(neighbor);
                    }

                    costSoFar.Add(neighbor, newCost);
                    cameFrom.Add(neighbor, current);
                    float priority = newCost + (neighbor.transform.position + destiny.transform.position).sqrMagnitude ;
                    frontier.Enqueue(neighbor, priority);
                }
            }
        }
        #endregion

        #region Return Path

        List<HexagonalTile> path = new List<HexagonalTile>();
        HexagonalTile aux = destiny;

        while (!aux.Equals(origin))
        {
            if (!cameFrom.ContainsKey(aux))
            {
                return new List<HexagonalTile>();
            }
            path.Add(aux);
            aux = cameFrom[aux];
        }

        path.Reverse();
        return path;

        #endregion
    }
    
    public class PriorityQueue<T>
    {
        private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, float priority)
        {
            elements.Add(new KeyValuePair<T, float>(item, priority));
        }

        // Returns the Location that has the lowest priority
        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Value < elements[bestIndex].Value)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Key;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

}