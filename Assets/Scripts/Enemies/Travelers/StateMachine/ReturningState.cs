using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturningState : AStateBehaviour
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
        traveler.Returning();
    }

    public override void OnStateEnd()
    {
        return;
    }

    public override int StateTransitionCondition()
    {
        float distanceToSpawn = Vector3.Distance(traveler.transform.position, traveler.spawnPoint);
        if (distanceToSpawn < 0.5f)
        {
            return (int)EEnemyState.Waiting;
        }
        return (int)EEnemyState.Invalid;
    }
}
