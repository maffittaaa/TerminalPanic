using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TravelerState
{
    Waiting
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


    //public GameObject SightZone;
    //public GameObject HearingZone;
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
        //add a variable for the collider
        //be on waiting mode
        //check if the player collided with that collider
        //if it did change to chase mode
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

                if (iSeePlayer) {
                    //currentState = TravelerState.chasing;
                }


            
                break;

                //case chasing:
                //destination = 
                  //  break;
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

