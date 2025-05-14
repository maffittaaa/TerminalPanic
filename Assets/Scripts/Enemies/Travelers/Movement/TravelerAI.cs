using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [Header("Traveller Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minChaseSpeed = 1.5f;
    [SerializeField] private float maxChaseSpeed = 5f;
    [SerializeField] private float slowDownDistance = 7f;
    [SerializeField] private float wanderRadius = 5f;

    [field: Header("Traveller State & Type")]
    [field: SerializeField] public TravelerState currentState { get; set; }
    [field: SerializeField] public TravelerType type { get; set; }
    [field: SerializeField] public IAirportMode currentMode { get; set; }

    [field: Header("Traveller Needs")]
    [SerializeField] private NavMeshAgent agent;
    [field: SerializeField] public static PlayerMovement player { get; set; }
    [field: SerializeField] public bool iHearPlayer { get; set; }
    [field: SerializeField] public bool iSeePlayer { get; set; }
    [field: SerializeField] public bool lostPlayer { get; set; }
    public Vector3 spawnPoint { get; set; }
    private Vector3 lastKnownPlayerPosition;
    public float lookAroundDuration { get; set; }
    public float lookAroundTimer { get; set; }
    private float chaseMemoryTime = 5f;
    private float chaseMemoryTimer = 0f;
    private float maxChaseDistance = 15f;
    private Vector3 wanderTarget;
    private float wanderTimer = 3f;
    private float wanderCounter;
    private float stateChangeTimer = 6f;
    private float stateChangeCounter = 0f;

    [Header("Traveller Audio")]
    [SerializeField] private AudioSource whispers;
    [SerializeField] private AudioSource highbass;
    [SerializeField] private AudioSource weird;

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        currentMode = IAirportMode.Panic;
        SetAudioSettings();
        SetAgentSettings();
    }

    private void SetAudioSettings()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.AddToAudioList(whispers);
        audioManager.AddToAudioList(highbass);
        audioManager.AddToAudioList(weird);
    }

    private void SetAgentSettings()
    {
        player = FindFirstObjectByType<PlayerMovement>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed * 100f;

        spawnPoint = transform.position;

        iSeePlayer = false;
        iHearPlayer = false;

        lookAroundDuration = 3f;
        lookAroundTimer = 0f;

        SetRandomNormalState();
        wanderCounter = wanderTimer;
    }

    private void Update()
    {
        if(gameManager.state != IGameStates.Paused && currentMode != IAirportMode.Normal)
        {
            if (!whispers.isPlaying || !highbass.isPlaying || !weird.isPlaying)
            {
                whispers.Play();
                highbass.Play();
                weird.Play();
            }
        }
        else
        {
            agent.speed = 0;
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

    public void OnAirportModeChanged(IAirportMode newMode)
    {
        StateMachine stateMachine = GetComponent<StateMachine>();
        stateMachine.type = newMode;
        currentMode = newMode;
/*        if (newMode == IAirportMode.Panic)
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
        else if (newMode == IAirportMode.Normal)
        {
            SetRandomNormalState();
        }*/
    }
}