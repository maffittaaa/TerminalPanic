using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private ThiefAStar thiefAStar;
    private Vector3 currentPosition;
    private Vector3 nextPosition;
    private float nextPositionZ;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float accuracy = 1f;
    [SerializeField] private ThiefHearingControl hearingZone;
    [SerializeField] private ThiefSightControl sightZone;
    private Rigidbody rb;
    public bool iHearPlayer;
    public bool iSeePlayer;
    
    [field: Header("Points To Go")]
    [SerializeField] private List<GameObject> randomPositionsToGo = new List<GameObject>();
    
    [field: Header("Scripts")]
    [SerializeField] private PlayerMovement player;
    
    [field: Header("Waypoints")]
    [SerializeField] private List<GameObject> waypoints = new List<GameObject>();
    private GameObject currentWaypoint;
    private int currentWaypointIndex = 0;
    
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        hearingZone = GetComponentInChildren<ThiefHearingControl>();
        sightZone = GetComponentInChildren<ThiefSightControl>();
        rb = GetComponent<Rigidbody>();

       GameObject[] tempWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
       GameObject[] tempPositions = GameObject.FindGameObjectsWithTag("NewPositions");
       foreach (GameObject wp in tempWaypoints)
           waypoints.Add(wp);
       foreach (GameObject rp in tempPositions)
            randomPositionsToGo.Add(rp);
        
        currentWaypoint = waypoints[0];
        StartState(ThiefState.Idle);
        currentPosition = transform.position;
    }
    
    public void StartState(ThiefState newState)
    {
        switch (newState)
        {
            case ThiefState.Fleeing:
                PickFurThestWaypointToGo();
                break;
            case ThiefState.Hiding:
                movementSpeed = 0f;
                break;
        }
        currentState = newState;
    }
    
    private void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        if (ThiefState.Fleeing == currentState)
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
    
    private void PickFurThestWaypointToGo()
    {
        float maximumDistance = 0;

        float currentDistance = 0;
        foreach (GameObject wp in waypoints)
        {
            currentDistance = Magnitude(transform.position - wp.transform.position);
            if (currentDistance > maximumDistance)
            {
                maximumDistance = currentDistance;
                currentWaypoint = wp;
            }
        }
        thiefAStar.SetWaypointAndGo(currentWaypoint);
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

    private void FleeingState()
    {
        if (Vector3.Distance(currentWaypoint.transform.position, transform.position) < accuracy) //if thief reaches waypoint, stays hidden
            StartState(ThiefState.Hiding);
    }

    private void HidingState()
    {
        StartCoroutine(WaitingToGoBack());
    }
    
    private IEnumerator WaitingToGoBack()
    {
        yield return new WaitUntil(() => Vector3.Distance(currentWaypoint.transform.position, transform.position) < accuracy);
        if (!iSeePlayer)
        {
            float y = transform.position.y;
            float minX = randomPositionsToGo[0].transform.position.x;
            float maxX = randomPositionsToGo[randomPositionsToGo.Count - 1].transform.position.x;
            float minZ = randomPositionsToGo[0].transform.position.z;
            float maxZ = randomPositionsToGo[randomPositionsToGo.Count - 1].transform.position.z;
            
            //randomize the next position that the thief needs to go to blend
            nextPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
            currentPosition = nextPosition;
            thiefAStar.MoveAlongPath();
        }
    }
}
