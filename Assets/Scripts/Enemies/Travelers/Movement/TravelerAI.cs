using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TravelerState
{
    Waiting,
    Chasing,
    Returning,
    Suspicious
}

public class TravelerAI : MonoBehaviour
{
    [SerializeField] private TravelerState currentState = TravelerState.Waiting;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minChaseSpeed = 1.5f;
    [SerializeField] private float maxChaseSpeed = 5f;
    [SerializeField] private float slowdownDistance = 7f;

    [SerializeField] private NavMeshAgent agent;
 
    public Vector3 spawnPoint;

    public static PlayerMovement player;


    public HearingControl hearingZone;
    public SightControl sightZone;

    public bool iHearPlayer;
    public bool iSeePlayer;

    private Vector3 lastKnownPlayerPosition;
    private float suspiciousWaitTime = 3f;
    private float suspiciousTimer = 0f;
    private float lookAroundDuration = 3f;
    private float lookAroundTimer = 0f;
    private float chaseMemoryTime = 5f;
    private float chaseMemoryTimer = 0f;
    private float maxChaseDistance = 15f;



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
        if (agent == null || player == null)
            return;

        if ((iHearPlayer || iSeePlayer) && currentState != TravelerState.Chasing)
        {
            SetState(TravelerState.Chasing);
            return;
        }

        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case TravelerState.Waiting:
                agent.isStopped = true;
                agent.speed = maxChaseSpeed;

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

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                float speedFactor = Mathf.Clamp01(distanceToPlayer / slowdownDistance);
                agent.speed = Mathf.Lerp(minChaseSpeed, maxChaseSpeed, speedFactor);

                agent.SetDestination(player.transform.position);

                if (iSeePlayer || iHearPlayer)
                {
                    chaseMemoryTimer = 0f;
                }
                else
                {
                    chaseMemoryTimer += Time.deltaTime;

                    if (distanceToPlayer < maxChaseDistance && chaseMemoryTimer < chaseMemoryTime)
                    {
                        agent.SetDestination(player.transform.position);
                    }
                    else
                    {
                        lastKnownPlayerPosition = player.transform.position;
                        SetState(TravelerState.Suspicious);
                        Debug.Log("Chase lost -> Entering Suspicious state.");
                    }
                }
                break;

            case TravelerState.Returning:
                agent.isStopped = false;
                agent.speed = maxChaseSpeed;

                agent.SetDestination(spawnPoint);

                float distanceToSpawn = Vector3.Distance(transform.position, spawnPoint);
                if (distanceToSpawn < 0.5f)
                {
                    SetState(TravelerState.Waiting);
                    Debug.Log("Traveler returned to spawn → Waiting.");
                }
                break;

            case TravelerState.Suspicious:
                agent.isStopped = false;
                agent.speed = maxChaseSpeed;

                agent.SetDestination(lastKnownPlayerPosition);

                float distanceToLastKnown = Vector3.Distance(transform.position, lastKnownPlayerPosition);

                if (distanceToLastKnown < 1f)
                {
                    agent.isStopped = true;
                    lookAroundTimer += Time.deltaTime;

                    float angle = Mathf.PingPong(Time.time * rotationSpeed * 20f, 90f) - 45f;
                    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + angle * Time.deltaTime, 0);

                    if (iSeePlayer || iHearPlayer)
                    {
                        SetState(TravelerState.Chasing);
                        Debug.Log("Player seen/heard during Suspicious → Chasing.");
                    }
                    else if (lookAroundTimer >= lookAroundDuration)
                    {
                        lookAroundTimer = 0f;
                        SetState(TravelerState.Returning);
                        Debug.Log("Finished looking around → Returning to spawn.");
                    }
                }
                break;


        }
    }


    public void SetState(TravelerState newState)
    {
        currentState = newState;

        if (newState == TravelerState.Suspicious)
        {
            suspiciousTimer = 0f;
            lookAroundTimer = 0f;
        }

        Debug.Log($"Traveler state changed to: {newState}");
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}

