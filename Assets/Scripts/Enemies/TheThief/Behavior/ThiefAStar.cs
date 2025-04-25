using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefAStar : MonoBehaviour
{
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject finish;
    [SerializeField] private GameObject label;
    private ThiefBehavior thiefBehavior;
    private AITileMapGenerator tileMapGenerator;
    private List<GameObject> path = new List<GameObject>();
    private Dictionary<Vector3, GameObject> floorTiles = new Dictionary<Vector3, GameObject>();


    private void Start()
    {
        thiefBehavior = gameObject.GetComponent<ThiefBehavior>();
        tileMapGenerator = gameObject.GetComponent<AITileMapGenerator>();
        AStart();
    }

    private void AStart()
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, float> costs = new Dictionary<GameObject, float>();
        PriorityQueue<GameObject> priorityQueue = new PriorityQueue<GameObject>();
        
        //start = tile that the thief starts;
        //finish = tile that the waypoint is;
        
        priorityQueue.Enqueue(start, 0);
        cameFrom[start] = null;
        costs[start] = 0;
        int i = 0;

        while (priorityQueue.Count > 0)
        {
            GameObject current = priorityQueue.Dequeue();

            if (GameObject.ReferenceEquals(current, finish))
            {
                ReconstructPath(cameFrom, finish);
                return;
            }

            List<GameObject> neighbors = GetNeighbors(current);

            foreach (GameObject neighbor in neighbors)
            {
                float newCost = costs[current] + GetCost(neighbor) + CalculateHeuristic(neighbor);
                if (!costs.ContainsKey(neighbor) || newCost < costs[neighbor])
                {
                    costs[neighbor] = newCost;
                    cameFrom[neighbor] = current;
                    
                    priorityQueue.EnqueueOrUpdate(neighbor, newCost);

                    // Instantiate the label at the neighbor�s position with a slight Y offset
                    //do we need this? just for debug right?
                    Vector3 labelPosition = neighbor.transform.position + new Vector3(0, 0.5f, 0);
                    GameObject numLabel = Instantiate(label, labelPosition, Quaternion.identity);
                    numLabel.GetComponent<TextMesh>().text = i.ToString();
                    
                    i++;
                }
            }
        }
    }
    
    private void ReconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject current)
    {
        while (current != null) 
        {
            path.Add(current);
            current = cameFrom[current]; 
        }

        path.Reverse();

        foreach (GameObject tile in path)
            tile.GetComponent<Renderer>().material.color = Color.green; 
    }
    
    private List<GameObject> GetNeighbors(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = RoundPosition(floorTile.transform.position, 1.0f);

        // Possible movement directions (now including up/down variations)
        //up and down variations are not needed, our map is flat
        Vector3[] directions = new Vector3[]
        {
            new Vector3(2, 0, 0),  // Right
            new Vector3(-2, 0, 0), // Left
            new Vector3(0, 0, 2),  // Forward
            new Vector3(0, 0, -2), // Backward
            
            // new Vector3(2, 2, 0),  // Up slope right
            // new Vector3(-2, 2, 0), // Up slope left
            // new Vector3(0, 2, 2),  // Up slope forward
            // new Vector3(0, 2, -2), // Up slope backward
            // new Vector3(2, -2, 0),  // Down slope right
            // new Vector3(-2, -2, 0), // Down slope left
            // new Vector3(0, -2, 2),  // Down slope forward
            // new Vector3(0, -2, -2)  // Down slope backward
        };
        
        //we don't need this, map is flat
        float yTolerance = 2f;

        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = RoundPosition(pos + dir, 1.0f);

            if (floorTiles.ContainsKey(neighborPos))
            {
                GameObject neighbor = floorTiles[neighborPos];
                
                //technically we don't need this part
                if (Mathf.Abs(neighbor.transform.position.y - pos.y) <= yTolerance)
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
    
    private Vector3 RoundPosition(Vector3 pos, float gridSize = 1.0f)
    {
        return new Vector3(
            Mathf.Round(pos.x / gridSize) * gridSize,
            Mathf.Round(pos.y / gridSize) * gridSize,
            Mathf.Round(pos.z / gridSize) * gridSize
        );
    }
    
    private int GetCost(GameObject tile)
    {
        int cost = 0;
        if (tile.CompareTag("Floor"))
            cost = 1;
        return cost;
    }

    private float CalculateHeuristic(GameObject tile)
    {
        float heuristics = 0;
        if (tile != null)
            heuristics = new Vector2(finish.transform.position.x - tile.transform.position.x, finish.transform.position.z - tile.transform.position.z).magnitude;
        return heuristics;
    }
}
