using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TravelerState
{
    Waiting,
    Chasing
}

public class TravelerAI : MonoBehaviour
{
    [SerializeField] private TravelerState currentState = TravelerState.Waiting;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject destination;
 
    private Transform spawnPoint;

    public static PlayerMovement player;


    public HearingControl hearingZone;
    public SightControl sightZone;

    public bool iHearPlayer;
    public bool iSeePlayer;





    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindFirstObjectByType<PlayerMovement>();
        }
        else
        {
            Debug.Log("Player is in.");
        }

        spawnPoint = gameObject.transform;
        //spawnPoint.position

        SetState(TravelerState.Waiting);
    }

    void FixedUpdate()
    {
        //if the player is far enough go back to the spawnPoint
        agent.SetDestination(destination.transform.position);

        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case TravelerState.Waiting:
                agent.isStopped = true;

                if (iSeePlayer)
                {
                    SetState(TravelerState.Chasing);
                    Debug.Log("Traveler sees the player ? chasing.");
                }

                else if (iHearPlayer)
                {
                    if (player != null && player.behaviorType != BehaviorType.Crouching)
                    {
                        SetState(TravelerState.Chasing);
                        Debug.Log("Traveler hears the player (not crouching) ? chasing.");
                    }
                }

                break;

            case TravelerState.Chasing:
                agent.isStopped = false;

                if (player != null)
                {
                    destination.transform.position = player.transform.position;
                    agent.SetDestination(destination.transform.position);
                }

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (distanceToPlayer > 15f && !iSeePlayer && !iHearPlayer)
                {
                    SetState(TravelerState.Waiting);
                }
                break;

        }
    }


    public void SetState(TravelerState newState)
    {
        currentState = newState;
        Debug.Log($"Traveler state changed to: {newState}");
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}

