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

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        MoveTraveler();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case TravelerState.Waiting:
                SetMoveDirection(Vector3.zero);
                break;

        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    private void MoveTraveler()
    {
        Vector3 targetVelocity = moveDirection * moveSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
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

