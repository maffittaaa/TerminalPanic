using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SearchType {BFS, DFS, UCS, AStar}

public class Search : MonoBehaviour
{
    [SerializeField] private SearchType searchType;
    [SerializeField] private AITileMapGenerator generator;

    public GameObject start;
    public GameObject finish;

    Dictionary<Vector3, GameObject> floorTiles = new Dictionary<Vector3, GameObject>();

    Queue<GameObject> queue = new Queue<GameObject>();
    List<GameObject> path = new List<GameObject>();
    private int currentTargetIndex = 0; 
    private float moveSpeed = 2f; 

    Vector3 RoundPosition(Vector3 pos, float gridSize = 1.0f)
    {
        return new Vector3(
            Mathf.Round(pos.x / gridSize) * gridSize,
            Mathf.Round(pos.y / gridSize) * gridSize,
            Mathf.Round(pos.z / gridSize) * gridSize
        );
    }
    

    void OnEnable()
    {
        if (start == null || finish == null)
            Debug.Log("Start and/or Finish are not defined!");

        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");

        foreach (GameObject floor in floors)
        {
            Vector3 roundedPos = RoundPosition(floor.transform.position, 1.0f);
            floorTiles[roundedPos] = floor;
        }

        switch(searchType)
        {
            case SearchType.BFS:
                BFS();
                break;
            case SearchType.DFS:
                DFS();
                break;
            case SearchType.UCS:
                UCS();
                break;
            case SearchType.AStar:
                AStar();
                break;
            default:
                searchType = SearchType.AStar;
                AStar();
                break;
        }
    }

    private void AStar()
    {
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

    private void UCS()
    {
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
                float newCost = costSoFar[current] + getCost(neighbor);

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
                if (Physics.Raycast(pos, dir, out hit, generator.size))
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

    private void BFS()
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        queue.Enqueue(start);
        cameFrom[start] = null;

        int i = 0;

        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();

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
                if (!cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);

                    i++; // Increment visit order counter
                }
            }
        }

        Debug.Log("No path found!");
    }

    private void DFS()
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(start);
        cameFrom[start] = null;

        int i = 0;

        while (stack.Count > 0)
        {
            GameObject current = stack.Pop();
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
                if (!cameFrom.ContainsKey(neighbor))
                {
                    stack.Push(neighbor);
                    cameFrom[neighbor] = current;

                    i++; // Increment visit order counter
                }
            }
        }

        Debug.Log("No path found!");
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
