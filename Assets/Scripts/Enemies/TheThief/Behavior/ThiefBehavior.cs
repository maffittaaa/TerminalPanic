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
    private Vector3 currentPosition;
    private float nextPositionX;
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
        Quaternion lookRotation = Quaternion.LookRotation(currentWaypoint.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, lookRotation.y, 0)), Time.deltaTime * rotationSpeed);
        Vector3 direction = currentWaypoint.transform.position - transform.position;
        direction = direction.normalized * movementSpeed;
        rb.AddForce(direction, ForceMode.Impulse);
                
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
        Debug.Log("Hello");
        if (!iSeePlayer)
        {
            //randomize the next position that the thief will be
            nextPositionX = Random.Range(randomPositionsToGo[0].transform.position.x, randomPositionsToGo[randomPositionsToGo.Count - 1].transform.position.x);
            nextPositionZ = Random.Range(randomPositionsToGo[0].transform.position.z, randomPositionsToGo[randomPositionsToGo.Count - 1].transform.position.z);
            currentPosition = new Vector3(nextPositionX, currentPosition.y, nextPositionZ);
            rb.AddForce(currentPosition * movementSpeed, ForceMode.Impulse);
        }
    }
}
