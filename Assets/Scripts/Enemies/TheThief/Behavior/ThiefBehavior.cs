using System;
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
    private Vector3 currentPosition;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int accuracy;
    [SerializeField] private HearingControl hearingZone;
    [SerializeField] private SightControl sightZone;
    private bool iHearPlayer;
    private bool iSeePlayer;
    
    [field: Header("Player")]
    [SerializeField] private PlayerMovement player;
    
    [field: Header("Waypoints")]
    [SerializeField] private List<GameObject> waypoints = new List<GameObject>();
    private GameObject currentWaypoint;
    private int currentWaypointIndex = 0;
    
    private void Start()
    {
        currentWaypoint = waypoints[0];
        State(ThiefState.Idle);
        currentPosition = transform.position;
    }

    private void Update()
    {
        SwitchBetweenStates();
    }

    private void SwitchBetweenStates()
    {
        switch (currentState)
        {
            case ThiefState.Idle:
                if (iSeePlayer)
                    State(ThiefState.Fleeing);
                else if (iHearPlayer)
                {
                    if (player.behaviorType != BehaviorType.Crouching)
                        State(ThiefState.Fleeing);
                }
                break;
            
            case ThiefState.Fleeing:
                if (currentWaypointIndex >= waypoints.Count - 1)
                    currentWaypointIndex = 0;
                
                Quaternion lookRotation = Quaternion.LookRotation(currentWaypoint.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                transform.Translate(0, 0, movementSpeed * Time.deltaTime);
                currentPosition = transform.position;

                if (transform.position == currentWaypoint.transform.position) //if thief reaches waypoint, stays hidden
                {
                    movementSpeed = 0f;
                    State(ThiefState.Hiding);
                }
                break;
            case ThiefState.Hiding:
                StartCoroutine(WaitingToGoBack());
                break;
        }
    }

    private IEnumerator WaitingToGoBack()
    {
        yield return new WaitUntil(() => iSeePlayer == false);
        // transform.Translate(Random.Range(currentPosition.x), movementSpeed * Time.deltaTime);
    }
    
    public void State(ThiefState newState)
    {
        currentState = newState;
    }
}
