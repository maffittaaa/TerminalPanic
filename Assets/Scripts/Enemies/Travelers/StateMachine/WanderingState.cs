using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : AStateBehaviour
{
    private TravelerAI traveler;

    public override bool InitializeState()
    {
        traveler = GetComponent<TravelerAI>();
        return true;
    }

    public override void OnStateStart()
    {

    }

    public override void OnStateUpdate()
    {


    }

    public override void OnStateFixedUpdate()
    {
        traveler.Wander();
    }

    public override void OnStateEnd()
    {

        return;
    }

    public override int StateTransitionCondition()
    {
        if(traveler.type == TravelerType.Sitters)
        {
            return (int) EEnemyState.Waiting;
        }

        if (traveler.iSeePlayer || traveler.iHearPlayer)
        {
            return (int) EEnemyState.Chasing;
        }
        return (int) EEnemyState.Invalid;
    }
}