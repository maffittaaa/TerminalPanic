using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ThiefState
{
    Idle,
    Fleeing,
    Hiding
}

public class ThiefBehavior : MonoBehaviour
{
    [field: Header("Thief")]
    [SerializeField] private ThiefState currentState;
    [SerializeField] private float rotationSpeed;
    public bool iHearPlayer;
    public bool iSeePlayer;
    
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
                PickFurThestWaypointsToGo();
                break;
            case ThiefState.Hiding:
                thiefAStar.speed = 0f;
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
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case ThiefState.Idle:
                IdleState();
                break;
            case ThiefState.Fleeing:
                FleeingState();
                if (Vector3.Distance(currentWaypoint.transform.position, transform.position) < thiefAStar.accuracy) //if thief reaches waypoint, stays hidden
                    StartState(ThiefState.Hiding);
                break;
            case ThiefState.Hiding:
                HidingState();
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
        if (currentWaypoint == waypoints[1] || currentWaypoint == waypoints[2] || currentWaypoint == waypoints[3] ||
            currentWaypoint == waypoints[4] || currentWaypoint == waypoints[5] || currentWaypoint == waypoints[6] ||
            currentWaypoint == waypoints[7] || currentWaypoint == waypoints[8] || currentWaypoint == waypoints[16] ||
            currentWaypoint == waypoints[17] || currentWaypoint == waypoints[18] || currentWaypoint == waypoints[19])
        {
            isHiding = true;
            Hide();
        }
        else
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
            thiefAStar.speed = 7f;
            int nextPositionIndex = Random.Range(0, randomPositionsToGo.Count);
            nextPosition = randomPositionsToGo[nextPositionIndex];
            thiefAStar.SetWhereToGo(nextPosition);
        }
        else
            thiefAStar.speed = 7f;
    }

    private void HidingState()
    {
        if (iSeePlayer) //if he sees the player while hiding
            StartState(ThiefState.Fleeing);
        if (Vector3.Distance(nextPosition.transform.position, transform.position) < thiefAStar.accuracy) //if he reaches the next position after hiding, he resets to idle
            StartState(ThiefState.Idle);
        if (isHiding)
        {
            if (iSeePlayer || iHearPlayer)
            {
                isHiding = false;
                StartState(ThiefState.Fleeing);
            }
        }
    }

    private void FleeingState()
    {
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
                closestHidingSpotPosition = hidePosition;
                minimumDistance = Vector3.Distance(transform.position, hideDistance);
            }
        }
        Debug.Log("Instantiating closest hiding spot");
        hidingSpotPrefab = Instantiate(hidingSpotPrefab, closestHidingSpotPosition, Quaternion.identity);
        closestHidingSpot = hidingSpotPrefab;
        thiefAStar.SetWhereToGo(closestHidingSpot);
    }
}
