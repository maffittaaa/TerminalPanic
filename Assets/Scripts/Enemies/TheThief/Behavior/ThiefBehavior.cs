using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ThiefState
{
    Idle,
    Fleeing,
    Hiding,
    MovingToAnotherPosition
}

public class ThiefBehavior : MonoBehaviour
{
    [field: Header("Thief")]
    [SerializeField] private ThiefState currentState;
    [SerializeField] private float rotationSpeed;
    public bool iHearPlayer;
    public bool iSeePlayer;
    [SerializeField] private Rigidbody rb;
    
    [field: Header("Points To Go")]
    [SerializeField] private List<GameObject> randomPositionsToGo = new List<GameObject>();
    private GameObject nextPosition;
    
    [field: Header("Waypoints")]
    [SerializeField] private List<GameObject> waypoints = new List<GameObject>();
    private Dictionary<GameObject, float> waypointDistances = new Dictionary<GameObject, float>();
    private GameObject currentWaypoint;
    
    [field: Header("HidingSpots")]
    private List<GameObject> hidingSpots = new List<GameObject>();
    [SerializeField] private GameObject hidingSpotPrefab;
    private GameObject currentHidingSpot;
    private Vector3 closestHidingSpotPosition;
    [SerializeField] private GameObject closestHidingSpot;
    private bool isHiding;
    
    [field: Header("Scripts")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private ThiefAStar thiefAStar;
    
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();

       GameObject[] tempWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
       GameObject[] tempPositions = GameObject.FindGameObjectsWithTag("NewPositions");
       GameObject[] tempHidingSpots = GameObject.FindGameObjectsWithTag("HidingSpot");
       foreach (GameObject wp in tempWaypoints)
           waypoints.Add(wp);
       foreach (GameObject rp in tempPositions)
            randomPositionsToGo.Add(rp);
       foreach(GameObject hs in tempHidingSpots)
           hidingSpots.Add(hs);
       currentWaypoint = waypoints[0];
       StartState(ThiefState.Idle);
    }
    
    public void StartState(ThiefState newState)
    {
        switch (newState)
        {
            case ThiefState.Fleeing:
                thiefAStar.speed = 5f;
                PickFurThestWaypointsToGo();
                break;
            case ThiefState.Hiding:
                thiefAStar.speed = 0f;
                if (!isHiding)
                    StartCoroutine(BlendIn());
                break;
        }
        currentState = newState;
    }

    private void FixedUpdate()
    {
        UpdateState();
        if (ThiefState.Fleeing == currentState)
            thiefAStar.MoveAlongPath();
        if (ThiefState.Hiding == currentState && !iSeePlayer && !isHiding)
            thiefAStar.MoveAlongPath();
        if (ThiefState.MovingToAnotherPosition == currentState && !isHiding)
            thiefAStar.MoveAlongPath();
        if (ThiefState.Fleeing == currentState && isHiding)
            thiefAStar.MoveAlongPath(); 
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case ThiefState.Idle:
                thiefAStar.speed = 0f;
                rb.velocity = Vector3.zero;
                IdleState();
                break;
            case ThiefState.Fleeing:
                FleeingState();
                break;
            case ThiefState.Hiding:
                HidingState();
                break;
            case ThiefState.MovingToAnotherPosition:
                MovingToAnotherPositionState();
                break;
        }
    }

    private float Magnitude(Vector3 vector)
    {
        return Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
    }
    
    private void PickFurThestWaypointsToGo()
    {
        float currentDistance;
        foreach (GameObject wp in waypoints)
        {
            currentDistance = Magnitude(transform.position - wp.transform.position);
            waypointDistances[wp] = currentDistance;
        }

        List<GameObject> sortedWaypoints = new List<GameObject>(waypointDistances.Keys); //created this list because you cant sort a dictionary directly
        sortedWaypoints.Sort((a, b) => waypointDistances[b].CompareTo(waypointDistances[a]));
        List<GameObject> threeFurthest = new List<GameObject>(); //this list stores the 3 furthest
        for (int i = 0; i < Mathf.Min(3, sortedWaypoints.Count); i++) //the Mathf.Min it's just to not get out of the index range basically, but it only increments until 3 because its minimum
            threeFurthest.Add(sortedWaypoints[i]);
        int randomWaypointIndex = Random.Range(0, threeFurthest.Count);
        currentWaypoint = threeFurthest[randomWaypointIndex];
        
        foreach (GameObject wp in waypoints)
        {
            if (currentWaypoint == wp)
            {
                if (wp.name.Contains("Hangar"))
                {
                    isHiding = true;
                    if (currentHidingSpot != null)
                        Destroy(currentHidingSpot);
                    currentHidingSpot = Instantiate(hidingSpotPrefab);
                    break;
                }
            }
        }
        if (!isHiding)
            thiefAStar.SetWhereToGo(currentWaypoint);
    }

    private void IdleState()
    {
        if (iSeePlayer)
            StartState(ThiefState.Fleeing);
        else if (iHearPlayer)
        {
            if (player.behaviorType != BehaviorType.Crouching)
                StartState(ThiefState.Fleeing);
        }
    }
    
    private IEnumerator BlendIn()
    {
        yield return new WaitForSeconds(3f);
        if (!iSeePlayer)
        {
            thiefAStar.speed = 5f;
            int nextPositionIndex = Random.Range(0, randomPositionsToGo.Count);
            nextPosition = randomPositionsToGo[nextPositionIndex];
            thiefAStar.SetWhereToGo(nextPosition);
            StartState(ThiefState.MovingToAnotherPosition);
        }
        else
            thiefAStar.speed = 5f;
    }

    private void HidingState()
    {
        if (iSeePlayer) //if he sees the player while hiding
            StartState(ThiefState.Fleeing);
    }

    private void MovingToAnotherPositionState()
    {
        if (Vector3.Distance(nextPosition.transform.position, transform.position) < thiefAStar.accuracy && !isHiding)//if he reaches the next position after hiding, he resets to idle
            StartState(ThiefState.Idle);
    }

    private void FleeingState()
    {
        if (isHiding)
        {
            Hide();
            if (iSeePlayer || iHearPlayer)
            {
                isHiding = false;
                StartState(ThiefState.Fleeing);
            }
        }
        if (Vector3.Distance(currentWaypoint.transform.position, transform.position) < thiefAStar.accuracy) //if thief reaches waypoint, stays hidden
            StartState(ThiefState.Hiding);
        if (!(Vector3.Distance(currentWaypoint.transform.position, transform.position) < thiefAStar.accuracy) && iSeePlayer) //while he is in fleeing mode to the waypoint and he sees the player, recalculate another waypoint to go
            StartState(ThiefState.Fleeing);
    }

    private void Hide()
    {
        float minimumDistance = Mathf.Infinity;
        
        Vector3 hideDistance;
        Vector3 hidePosition;
        foreach (GameObject hs in hidingSpots)
        {
            hideDistance = hs.transform.position - player.transform.position;
            hidePosition = hs.transform.position + hideDistance.normalized * 5;
            if (Vector3.Distance(transform.position, hideDistance) < minimumDistance)
            {
                closestHidingSpotPosition.x = hidePosition.x;
                closestHidingSpotPosition.z = hidePosition.z;
                closestHidingSpotPosition.y = 19f;
                minimumDistance = Vector3.Distance(transform.position, hideDistance);
            }
        }
        currentHidingSpot.transform.position = closestHidingSpotPosition;
        closestHidingSpot = currentHidingSpot;
        thiefAStar.SetWhereToGo(closestHidingSpot);
    }
}
