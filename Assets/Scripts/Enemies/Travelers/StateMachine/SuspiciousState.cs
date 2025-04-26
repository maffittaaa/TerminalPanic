using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspicious : AStateBehaviour
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
        traveler.Chase();
    }

    public override void OnStateEnd()
    {

        return;
    }

    public override int StateTransitionCondition()
    {
        if (traveler.iSeePlayer || traveler.iHearPlayer)
        {
            return (int) EEnemyState.Chasing;
        }
        else if (traveler.lookAroundTimer >= traveler.lookAroundDuration)
        {
            traveler.lookAroundTimer = 0f;
            return (int) EEnemyState.Returning;
        }

        return (int)EEnemyState.Invalid;
    }
}
