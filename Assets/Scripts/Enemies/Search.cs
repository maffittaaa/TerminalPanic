using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : MonoBehaviour
{
    [SerializeField] private AITileMapGenerator generator;
    [SerializeField] private TileMapTile player;

    public GameObject start;
    public GameObject finish;

    Dictionary<Vector3, GameObject> floorTiles = new Dictionary<Vector3, GameObject>();

    List<GameObject> path = new List<GameObject>();

    Vector3 RoundPosition(Vector3 pos, float gridSize = 1.0f)
    {
        return new Vector3(
            Mathf.Round(pos.x / gridSize) * gridSize,
            Mathf.Round(pos.y / gridSize) * gridSize,
            Mathf.Round(pos.z / gridSize) * gridSize
        );
    }
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TileMapTile>();

        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");

        foreach (GameObject floor in floors)
        {
            Vector3 roundedPos = RoundPosition(floor.transform.position, 1.0f);
            floorTiles[roundedPos] = floor;
        }

        player.tileChanged.AddListener(AStar);
    }

    private void AStar()
    {
        ClearPath();
        finish = player.currentTile;
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, float> costSoFar = new Dictionary<GameObject, float>();
        PriorityQueue<GameObject> priorityQueue = new PriorityQueue<GameObject>();
        priorityQueue.Enqueue(start, 0);
        cameFrom[start] = null;
        costSoFar[start] = 0;

        int i = 0;

        while (priorityQueue.Count > 0)
        {
            GameObject current = priorityQueue.Dequeue();

            Debug.Log("CURRENT = " + current.name);

            if (GameObject.ReferenceEquals(current, finish))
            {
                Debug.Log("Path Found!");
                ReconstructPath(cameFrom, finish);
                return;
            }

            List<GameObject> neighbors = GetNeighbors(current);

            foreach (GameObject neighbor in neighbors)
            {
                float newCost = costSoFar[current] + getCost(neighbor) + CalculateHeuristics(neighbor);

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    cameFrom[neighbor] = current;

                    priorityQueue.EnqueueOrUpdate(neighbor, newCost);

                    i++; // Increment visit order counter
                }
            }
        }

        Debug.Log("No path found!");
    }

    private void ClearPath()
    {
        foreach (GameObject tile in path)
        {
            tile.GetComponent<Renderer>().material.color = Color.white;
        }
        path.Clear();
    }

    private int getCost(GameObject tile)
    {
        int cost = 0;

        if (tile != null) 
        {
            cost = 1;
//            tile.GetComponent<Renderer>().material.color = Color.white;
        }

        return cost;
    }

    private float CalculateHeuristics(GameObject currentTile)
    {
        float heuristics = 0;

        if (currentTile != null) 
        {
            heuristics = new Vector2(finish.transform.position.x - currentTile.transform.position.x, finish.transform.position.z - currentTile.transform.position.z).magnitude;
        }

        return heuristics;
    }

    List<GameObject> GetNeighbors(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = RoundPosition(floorTile.transform.position, 1.0f);

        // Possible movement directions (now including up/down variations)
        Vector3[] directions = new Vector3[]
        {
            new Vector3(0, 0, generator.size),  // Forward
            new Vector3(0, 0, -generator.size), // Backward
            new Vector3(generator.size, 0, 0),  // Right
            new Vector3(-generator.size, 0, 0), // Left
        };

        float yTolerance = 2f; // Maximum allowed height difference

        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = RoundPosition(pos + dir, 1.0f);

            if (floorTiles.ContainsKey(neighborPos))
            {
                GameObject neighbor = floorTiles[neighborPos];

                if (Mathf.Abs(neighbor.transform.position.y - pos.y) <= yTolerance)
                {
                    neighbors.Add(neighbor);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(pos, dir, out hit, generator.size, LayerMask.GetMask("AITilemap")))
                {
                    if (floorTiles.ContainsKey(RoundPosition(hit.collider.gameObject.transform.position, 1.0f)) && !neighbors.Contains(hit.collider.gameObject))
                    {
                        GameObject neighbor = hit.collider.gameObject;
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    void ReconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject current)
    {
        while (current != null) 
        {
            path.Add(current);
            current = cameFrom[current]; 
        }

        path.Reverse(); // Reverse to get start -> goal order
        
        Debug.Log("Path Length: " + path.Count);
        foreach (GameObject tile in path)
        {
            tile.GetComponent<Renderer>().material.color = Color.red; 
        }
    }
}
