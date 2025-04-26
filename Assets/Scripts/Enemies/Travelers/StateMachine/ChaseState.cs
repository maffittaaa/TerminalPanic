using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChaseState : AStateBehaviour
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
        if (traveler.lostPlayer)
        {
            return (int) EEnemyState.Suspicious;
        }
        return (int) EEnemyState.Invalid;
    }
}
