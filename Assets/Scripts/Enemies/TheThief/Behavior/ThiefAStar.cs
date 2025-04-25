using System.Collections.Generic;
using UnityEngine;

public class ThiefAStar : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int currentTargetIndex;
    [SerializeField] private float tileSize;
    [SerializeField] private TileMapTile player;

    public GameObject currentTile;
    public GameObject waypointTile;

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

        SetCurrentTile();
        player.tileChanged.AddListener(AStarPathFinding);
    }

    private void AStarPathFinding()
    {
        ClearPath();
        waypointTile = player.currentTile;
        SetCurrentTile();
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, float> costSoFar = new Dictionary<GameObject, float>();
        PriorityQueue<GameObject> priorityQueue = new PriorityQueue<GameObject>();
        priorityQueue.Enqueue(currentTile, 0);
        cameFrom[currentTile] = null;
        costSoFar[currentTile] = 0;

        while (priorityQueue.Count > 0)
        {
            GameObject current = priorityQueue.Dequeue();

            //Debug.Log("CURRENT = " + current.name);

            if (GameObject.ReferenceEquals(current, waypointTile))
            {
                //Debug.Log("Path Found!");
                ReconstructPath(cameFrom, waypointTile);
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
                }
            }
        }

        //Debug.Log("No path found!");
    }

    private void SetCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, transform.localScale.y, LayerMask.GetMask("AITilemap")))
        {
            currentTile = hit.collider.gameObject;
        }
    }


    private void ClearPath()
    {
        foreach (GameObject tile in path)
        {
            //tile.GetComponent<Renderer>().material.color = Color.white;
        }
        currentTargetIndex = 1;
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
            heuristics = new Vector2(waypointTile.transform.position.x - currentTile.transform.position.x, waypointTile.transform.position.z - currentTile.transform.position.z).magnitude;
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
            new Vector3(0, 0, tileSize),  // Forward
            new Vector3(0, 0, -tileSize), // Backward
            new Vector3(tileSize, 0, 0),  // Right
            new Vector3(-tileSize, 0, 0), // Left
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
                if (Physics.Raycast(pos, dir, out hit, tileSize, LayerMask.GetMask("AITilemap")))
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
        
        //Debug.Log("Path Length: " + path.Count);
/*        foreach (GameObject tile in path)
        {
            tile.GetComponent<Renderer>().material.color = Color.red; 
        }*/
    }

    private void MoveAlongPath()
    {
        if (currentTargetIndex >= path.Count) return;

        // Get target tile position
        Vector3 targetPos = path[currentTargetIndex].transform.position;
        targetPos.y = transform.parent.position.y;

        Vector3 direction = (player.transform.position - transform.parent.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, targetRotation, 10 * Time.deltaTime);
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < tileSize)
        {
            currentTargetIndex++;
        }
        Debug.DrawRay(transform.position, targetPos, Color.magenta, tileSize / 2);
    }

    void FixedUpdate()
    {
        if (currentTile && waypointTile)
        {
            MoveAlongPath();
        }
    }
}

