using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TravelerState
{
    Waiting,
    Chasing,
    Returning
}

public class TravelerAI : MonoBehaviour
{
    [SerializeField] private TravelerState currentState = TravelerState.Waiting;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private NavMeshAgent agent;
    //[SerializeField] private GameObject destination;
 
    public Vector3 spawnPoint;

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
            if (player == null)
                Debug.LogWarning("TravelerAI: Player not found in scene.");
        }
        else
        {
            Debug.Log("Player is in.");
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
                Debug.LogError("TravelerAI: No NavMeshAgent found on traveler.");
        }


        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.acceleration = acceleration;
            agent.angularSpeed = rotationSpeed * 100f;
        }

        spawnPoint = transform.position;

        SetState(TravelerState.Waiting);
    }

    void FixedUpdate()
    {
        //if the player is far enough go back to the spawnPoint
        //agent.SetDestination(destination.transform.position);
        if (agent == null || player == null)
            return;

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
                    Debug.Log("Traveler sees the player -> chasing.");
                }

                else if (iHearPlayer)
                {
                    if (player != null && player.behaviorType != BehaviorType.Crouching)
                    {
                        SetState(TravelerState.Chasing);
                        Debug.Log("Traveler hears the player (not crouching) -> chasing.");
                    }
                }

                break;

            case TravelerState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer > 15f && !iSeePlayer && !iHearPlayer)
                {
                    SetState(TravelerState.Returning);
                    Debug.Log("Player escaped → Returning to spawn.");
                }
                break;

            case TravelerState.Returning:
                agent.isStopped = false;
                agent.SetDestination(spawnPoint);

                float distanceToSpawn = Vector3.Distance(transform.position, spawnPoint);
                if (distanceToSpawn < 0.5f)
                {
                    SetState(TravelerState.Waiting);
                    Debug.Log("Traveler returned to spawn → Waiting.");
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

