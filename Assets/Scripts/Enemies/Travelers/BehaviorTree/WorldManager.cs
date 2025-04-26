using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldManager : MonoBehaviour
{
    [SerializeField] private TravelerAI traveller;
    [field: SerializeField] public bool playerInSight { get; set; }
    [field: SerializeField] public bool playerLost { get; set; }

    private void Start()
    {
        traveller = GetComponent<TravelerAI>();
    }

    public void ChasePlayer()
    {
        if (IsPlayerInSight() && traveller.currentState != TravelerState.Chasing)
        {
            traveller.SetState(TravelerState.Chasing);
        }
    }

    public void Wander()
    {
        traveller.SetState(TravelerState.Wandering);
    }

    public void Wait()
    {
        traveller.SetState(TravelerState.Waiting);
    }

    public void Suspect()
    {
        traveller.SetState(TravelerState.Suspicious);
    }

    public void Return()
    {
        traveller.SetState(TravelerState.Returning);
    }

    public bool IsPlayerInSight()
    {
        if (traveller.iSeePlayer || traveller.iHearPlayer)
        {
            traveller.lostPlayer = false;
            return true;
        }

        return false;
    }

    public bool PlayerLost()
    {
        return traveller.lostPlayer;
    }    
    
    public bool IsSitter()
    {
        return traveller.type == TravelerType.Sitters;
    }    
    
    public bool IsWander()
    {
        return traveller.type == TravelerType.Wanders;
    }
}