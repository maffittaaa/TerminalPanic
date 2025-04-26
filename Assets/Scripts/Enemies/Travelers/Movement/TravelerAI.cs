using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TravelerState
{
    Waiting,
    Chasing,
    Returning,
    Suspicious,
    Wandering // for normal mode, differs from steering behavior 
}
public enum TravelerType
{
    Sitters,
    Wanders
}

public class TravelerAI : MonoBehaviour
{
    [SerializeField] private TravelerState currentState = TravelerState.Waiting;
    [field:SerializeField] public TravelerType type { get; set; }
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minChaseSpeed = 1.5f;
    [SerializeField] private float maxChaseSpeed = 5f;
    [SerializeField] private float slowdownDistance = 7f;
    [SerializeField] private float detectionDelay = 1f;

    private float detectionDelayTimer;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public Vector3 spawnPoint;
    [SerializeField] public static PlayerMovement player;


    [SerializeField] public HearingControl hearingZone;
    [SerializeField] public SightControl sightZone;

    [SerializeField] public bool iHearPlayer;
    [SerializeField] public bool iSeePlayer;

    private Vector3 lastKnownPlayerPosition;
    private float suspiciousWaitTime = 3f;
    private float suspiciousTimer = 0f;
    private float lookAroundDuration = 3f;
    private float lookAroundTimer = 0f;
    private float chaseMemoryTime = 5f;
    private float chaseMemoryTimer = 0f;
    private float maxChaseDistance = 15f;

    private TravelerSpawner spawner;

    private Vector3 wanderTarget;
    [SerializeField] public float wanderRadius = 5f;
    private float wanderTimer = 3f;
    private float wanderCounter;

    private float stateChangeTimer = 6f;
    private float stateChangeCounter = 0f;


    public void InitWithSpawner(TravelerSpawner spawner)
    {
        this.spawner = spawner;
    }

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindFirstObjectByType<PlayerMovement>();
            //if (player == null)
                //Debug.LogWarning("TravelerAI: Player not found in scene.");
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            //if (agent == null)
                //Debug.LogError("TravelerAI: No NavMeshAgent found on traveler.");
        }


        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.acceleration = acceleration;
            agent.angularSpeed = rotationSpeed * 100f;
        }

        spawnPoint = transform.position;
        detectionDelayTimer = detectionDelay;

        iSeePlayer = false;
        iHearPlayer = false;

        SetRandomNormalState();
        wanderCounter = wanderTimer;
        SetNewWanderTarget();
    }

    void FixedUpdate()
    {
        if (agent == null || player == null)
            return;

        if (spawner.currentMode == AirportMode.Normal)
        {
            UpdateStateOverTime();

            if (currentState == TravelerState.Wandering)
            {
                WanderBehavior();
            }
            else if (currentState == TravelerState.Waiting)
            {
                agent.isStopped = true;
            }

            return;
        }

        if (detectionDelayTimer > 0f)
        {
            detectionDelayTimer -= Time.deltaTime;
            return;
        }

        if ((iHearPlayer || iSeePlayer) && currentState != TravelerState.Chasing)
        {
            SetState(TravelerState.Chasing);
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
                    //Debug.Log("Traveler sees the player -> chasing.");
                }

                else if (iHearPlayer)
                {
                    if (player != null && player.behaviorType != BehaviorType.Crouching)
                    {
                        SetState(TravelerState.Chasing);
                        //Debug.Log("Traveler hears the player (not crouching) -> chasing.");
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
                        //Debug.Log("Chase lost -> Entering Suspicious state.");
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
                    //Debug.Log("Traveler returned to spawn → Waiting.");
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
                        //Debug.Log("Player seen/heard during Suspicious → Chasing.");
                    }
                    else if (lookAroundTimer >= lookAroundDuration)
                    {
                        lookAroundTimer = 0f;
                        SetState(TravelerState.Returning);
                        //Debug.Log("Finished looking around → Returning to spawn.");
                    }
                }
                break;

            case TravelerState.Wandering:
                WanderBehavior();
                break;
        }
    }

    void WanderBehavior()
    {
        wanderCounter -= Time.deltaTime;

        if (wanderCounter <= 0f)
        {
            SetNewWanderTarget();
            wanderCounter = wanderTimer;
        }

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = minChaseSpeed;
            agent.SetDestination(wanderTarget);
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

        if (newState == TravelerState.Chasing)
        {
            agent.isStopped = false; 
        }

        //Debug.Log($"Traveler state changed to: {newState}");
    }

    void SetNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection.y = 0f;
        wanderTarget = spawnPoint + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(wanderTarget, out hit, wanderRadius, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;
        }
    }

    public void SetRandomNormalState()
    {
        if (Random.value > 0.5f)
            SetState(TravelerState.Wandering);
        else
            SetState(TravelerState.Waiting);
    }

    public void UpdateStateOverTime()
    {
        stateChangeCounter += Time.deltaTime;
        if (stateChangeCounter >= stateChangeTimer)
        {
            stateChangeCounter = 0f;
            SetRandomNormalState();
        }
    }

    public void OnAirportModeChanged(AirportMode newMode)
    {
        if (newMode == AirportMode.Panic)
        {
            if (type == TravelerType.Sitters)
            {
                SetState(TravelerState.Waiting);
            }
            else if(type == TravelerType.Wanders)
            {
                SetState(TravelerState.Wandering);
            }
        }
        else if (newMode == AirportMode.Normal)
        {
            SetRandomNormalState();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        Debug.Log("Player has been caught by traveler.");

        //Destroy(player.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something triggered me: " + other.gameObject.name);

        if (spawner.currentMode == AirportMode.Panic && currentState == TravelerState.Chasing && other.CompareTag("Player"))
        {
            //Debug.Log("Traveler caught the player. (in panic mode)");
            KillPlayer();
        }
    }

}

