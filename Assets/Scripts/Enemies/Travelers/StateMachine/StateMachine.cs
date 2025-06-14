using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private List<AStateBehaviour> stateBehaviours = new List<AStateBehaviour>();
    public IAirportMode type { get; set; }

    [SerializeField] private int defaultState = 0;

    private AStateBehaviour currentState = null;

    [Header("Managers")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TravelerAI traveller;


    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        type = IAirportMode.Panic;

        if (!InitializeStates())
        {
            this.enabled = false;
            return;
        }

        if (stateBehaviours.Count > 0)
        {
            int firstStateIndex = defaultState < stateBehaviours.Count ? defaultState : 0;

            currentState = stateBehaviours[firstStateIndex];
            currentState.OnStateStart();
        }
        else
        {
            Debug.Log($"StateMachine On {gameObject.name} has no state behaviours associated with it!");
        }
    }
    private bool InitializeStates()
    {
        for (int i = 0; i < stateBehaviours.Count; ++i)
        {
            AStateBehaviour stateBehaviour = stateBehaviours[i];
            if (stateBehaviour && stateBehaviour.InitializeState())
            {
                stateBehaviour.AssociatedStateMachine = this;
                continue;
            }

            Debug.Log($"StateMachine On {gameObject.name} has failed to initialize the state {stateBehaviours[i]?.GetType().Name}!");
            return false;
        }

        return true;
    }

    private void Update()
    {
        currentState.OnStateUpdate();

        int newState = currentState.StateTransitionCondition();
        if (IsValidNewStateIndex(newState))
        {
            currentState.OnStateEnd();
            currentState = stateBehaviours[newState];
            currentState.OnStateStart();
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.state != IGameStates.Paused && type != IAirportMode.Normal && !traveller.dead)
        {
            currentState.OnStateFixedUpdate();

            int newState = currentState.StateTransitionCondition();
            if (IsValidNewStateIndex(newState))
            {
                currentState.OnStateEnd();
                currentState = stateBehaviours[newState];
                currentState.OnStateStart();
            }
        }
    }

    public bool IsCurrentState(AStateBehaviour stateBehaviour)
    {
        return currentState == stateBehaviour;
    }

    public void SetState(int index)
    {
        if (IsValidNewStateIndex(index))
        {
            currentState.OnStateEnd();
            currentState = stateBehaviours[index];
            currentState.OnStateStart();
        }
    }

    private bool IsValidNewStateIndex(int stateIndex)
    {
        return stateIndex < stateBehaviours.Count && stateIndex >= 0;
    }

    public AStateBehaviour GetCurrentState()
    {
        return currentState;
    }
}