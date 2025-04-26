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
    Wandering
}
public enum TravelerType
{
    Sitters,
    Wanders
}

public class TravelerAI : MonoBehaviour
{
    [SerializeField] public TravelerState currentState { get; set; }
    [field:SerializeField] public TravelerType type { get; set; }

    [Header("Traveller Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minChaseSpeed = 1.5f;
    [SerializeField] private float maxChaseSpeed = 5f;
    [SerializeField] private float slowDownDistance = 7f;
    [SerializeField] private float wanderRadius = 5f;

    [Header("Traveller Needs")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public Vector3 spawnPoint { get; set; }
    [SerializeField] public static PlayerMovement player { get; set; }
    [SerializeField] public bool iHearPlayer { get; set; }
    [SerializeField] public bool iSeePlayer { get; set; }
    [SerializeField] public bool lostPlayer { get; set; }

    private Vector3 lastKnownPlayerPosition;
    [HideInInspector] public float lookAroundDuration { get; set; }
    [HideInInspector] public float lookAroundTimer { get; set; }
    private float chaseMemoryTime = 5f;
    private float chaseMemoryTimer = 0f;
    private float maxChaseDistance = 15f;
    private TravelerSpawner spawner;
    private Vector3 wanderTarget;
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
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.angularSpeed = rotationSpeed * 100f;
        }

        spawnPoint = transform.position;

        iSeePlayer = false;
        iHearPlayer = false;

        lookAroundDuration = 3f;
        lookAroundTimer = 0f;

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
                Wander();
            }
            else if (currentState == TravelerState.Waiting)
            {
                agent.isStopped = true;
            }

            return;
        }
    }

    public void Waiting()
    {
        agent.isStopped = true;
        agent.speed = maxChaseSpeed;
    }

    public void Chase()
    {
        agent.isStopped = false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        float speedFactor = Mathf.Clamp01(distanceToPlayer / slowDownDistance);
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
                lostPlayer = true;
            }
        }
    }

    public void Returning()
    {
        agent.isStopped = false;
        agent.speed = maxChaseSpeed;

        agent.SetDestination(spawnPoint);
    }

    public void Suspicious()
    {
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
        }
    }

    public void Wander()
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
            lookAroundTimer = 0f;
        }

        if (newState == TravelerState.Chasing)
        {
            agent.isStopped = false; 
        }
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
            WorldInteractions player = FindFirstObjectByType<WorldInteractions>();

            KillPlayer(player);
        }
    }

    void KillPlayer(WorldInteractions player)
    {
        player.KillPlayer();
    }
}