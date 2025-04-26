using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEnemyState
{
    Invalid = -1,
    Wandering = 0,
    Waiting = 1,
    Chasing = 2,
    Suspicious = 3,
    Returning = 4,
}

public abstract class AStateBehaviour : MonoBehaviour
{
    public StateMachine AssociatedStateMachine { get; set; }
    public abstract bool InitializeState();
    public abstract void OnStateStart();
    public abstract void OnStateUpdate();
    public abstract void OnStateFixedUpdate();
    public abstract void OnStateEnd();
    public abstract int StateTransitionCondition();
}
