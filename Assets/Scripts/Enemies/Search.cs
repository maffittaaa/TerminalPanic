using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SearchType {BFS, DFS, UCS, AStar}

public class Search : MonoBehaviour
{
    [SerializeField] private SearchType searchType;

    public GameObject start;
    public GameObject finish;
    public Material start_material;
    public Material finish_material;
    public Material debug_material;
    public Material path_material;
    public GameObject label;
    public GameObject player;

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
    

    void Start()
    {

        if (start == null || finish == null)
            Debug.Log("Start and/or Finish are not defined!");
        else
        {
            // change color
            start.GetComponent<Renderer>().material = new Material(start_material);
            finish.GetComponent<Renderer>().material = new Material(finish_material);
        }

        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");

        foreach (GameObject floor in floors)
        {
            Vector3 roundedPos = RoundPosition(floor.transform.position, 1.0f);
            floorTiles[roundedPos] = floor;
        }

        GameObject[] bridges = GameObject.FindGameObjectsWithTag("Bridge");

        foreach (GameObject bridge in bridges)
        {
            Vector3 roundedPos = RoundPosition(bridge.transform.position, 1.0f);
            floorTiles[roundedPos] = bridge;
        }

        DebugNeighbours();

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
            current.GetComponent<Renderer>().material = new Material(path_material);

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

                    // Instantiate the label at the neighbor’s position with a slight Y offset
                    Vector3 labelPosition = neighbor.transform.position + new Vector3(0, 0.5f, 0);
                    GameObject numLabel = Instantiate(label, labelPosition, Quaternion.identity);
                    numLabel.GetComponent<TextMesh>().text = i.ToString();

                    // Ensure the label always faces the camera
                    numLabel.AddComponent<Billboard>();

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
            current.GetComponent<Renderer>().material = new Material(path_material);

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

                    // Instantiate the label at the neighbor’s position with a slight Y offset
                    Vector3 labelPosition = neighbor.transform.position + new Vector3(0, 0.5f, 0);
                    GameObject numLabel = Instantiate(label, labelPosition, Quaternion.identity);
                    numLabel.GetComponent<TextMesh>().text = i.ToString();

                    // Ensure the label always faces the camera
                    numLabel.AddComponent<Billboard>();

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
            if (tile.tag.Contains("TrapDoor"))
            {
                cost = 8;
                tile.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (tile.tag.Contains("SpikeTrap"))
            {
                cost = 6;
                tile.GetComponent<Renderer>().material.color = new Color(1, 0.624f, 0);
            }
            else if (tile.tag.Contains("Grate"))
            {
                cost = 4;
                tile.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (tile.tag.Contains("Bridge"))
            {
                cost = 2;
                tile.GetComponent<Renderer>().material.color = Color.green;
            }
            else if (tile.tag.Contains("Floor"))
            {
                cost = 1;
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
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

    List<GameObject> GetNeighbors2(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = RoundPosition(floorTile.transform.position, 1.0f);

        Vector3[] directions = new Vector3[]
        {
        new Vector3(2, 0, 0),
        new Vector3(-2, 0, 0),
        new Vector3(0, 0, 2),
        new Vector3(0, 0, -2),
        };

        foreach (Vector3 dir in directions)
        {
            Vector3 neighborPos = RoundPosition(pos + dir, 1.0f);
            if (floorTiles.ContainsKey(neighborPos))
            {
                neighbors.Add(floorTiles[neighborPos]);
            }
        }

        Debug.Log(neighbors.Count);
        return neighbors;
    }

    List<GameObject> GetNeighbors(GameObject floorTile)
    {
        List<GameObject> neighbors = new List<GameObject>();
        Vector3 pos = RoundPosition(floorTile.transform.position, 1.0f);

        // Possible movement directions (now including up/down variations)
        Vector3[] directions = new Vector3[]
        {
        new Vector3(0, 0, 2),  // Forward
        new Vector3(0, 0, -2), // Backward
        new Vector3(2, 2, 0),  // Up slope right
        new Vector3(-2, 2, 0), // Up slope left
        new Vector3(0, 2, 2),  // Up slope forward
        new Vector3(0, 2, -2), // Up slope backward
        new Vector3(2, -2, 0),  // Down slope right
        new Vector3(-2, -2, 0), // Down slope left
        new Vector3(0, -2, 2),  // Down slope forward
        new Vector3(0, -2, -2),  // Down slope backward
        new Vector3(2, 0, 0),  // Right
        new Vector3(-2, 0, 0), // Left

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
        }

        return neighbors;
    }

    private void DebugNeighbours()
    {
        List<GameObject> neighbours = GetNeighbors(start);
        foreach(GameObject n in neighbours)
        {
            n.GetComponent<Renderer>().material = new Material(debug_material);
        }

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
            current.GetComponent<Renderer>().material = new Material(path_material);

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
                    cameFrom[neighbor] = current;

                    // Instantiate the label at the neighbor’s position with a slight Y offset
                    Vector3 labelPosition = neighbor.transform.position + new Vector3(0, 0.5f, 0);
                    GameObject numLabel = Instantiate(label, labelPosition, Quaternion.identity);
                    numLabel.GetComponent<TextMesh>().text = i.ToString();

                    // Ensure the label always faces the camera
                    numLabel.AddComponent<Billboard>();

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
            current.GetComponent<Renderer>().material = new Material(path_material);

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

                    // Instantiate the label at the neighbor’s position with a slight Y offset
                    Vector3 labelPosition = neighbor.transform.position + new Vector3(0, 0.5f, 0);
                    GameObject numLabel = Instantiate(label, labelPosition, Quaternion.identity);
                    numLabel.GetComponent<TextMesh>().text = i.ToString();

                    // Ensure the label always faces the camera
                    numLabel.AddComponent<Billboard>();

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
            tile.GetComponent<Renderer>().material.color = Color.green; 
        }
    }

    void MoveAlongPath()
    {
        if (currentTargetIndex >= path.Count) return; 

        // Get target tile position
        Vector3 targetPos = path[currentTargetIndex].transform.position;

        targetPos.y = targetPos.y + path[currentTargetIndex].transform.localScale.y + player.transform.localScale.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            currentTargetIndex++; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && path.Count > 0)
        {
            MoveAlongPath();
        }
    }
}
