using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private WorldManager worldManager; 
    private Task behaviorTree; 
    private TaskStatus behaviorTreeStatus = TaskStatus.NONE;

    public float moveSpeed = 1f;

    private void Start() 
    {
        behaviorTree = new Selector();

        Task sitters = new Selector();
        Task wanders = new Selector();

        Task sittersChase = new Sequence();

        Task wandersChase = new Sequence();

        Task chasePlayerSequence = new Sequence();
        Task lostPlayer = new Sequence();

        sitters.AddChildren(sittersChase);
        sitters.AddChildren(lostPlayer);

        wanders.AddChildren(wandersChase);
        wanders.AddChildren(lostPlayer);

        sittersChase.AddChildren(new BehaviorConditions("IsSitter", ConditionType.IS_SITTER));
        sittersChase.AddChildren(new BehaviorConditions("SeePlayer", ConditionType.CHECK_PLAYER_SIGHT));
        sittersChase.AddChildren(chasePlayerSequence);

        wandersChase.AddChildren(new BehaviorConditions("IsWander", ConditionType.IS_WANDER));
        wandersChase.AddChildren(new BehaviorConditions("SeePlayer", ConditionType.CHECK_PLAYER_SIGHT));
        wandersChase.AddChildren(chasePlayerSequence);

        lostPlayer.AddChildren(new BehaviorConditions("LostPlayer", ConditionType.LOST_PLAYER));
        lostPlayer.AddChildren(new BehaviorActions("Suspicious", ActionType.SUSPICIOUS));
        lostPlayer.AddChildren(new BehaviorActions("Return", ActionType.RETURN));

        chasePlayerSequence.AddChildren(new BehaviorActions("ChasePlayer", ActionType.CHASE));

        behaviorTree.AddChildren(sitters);
        behaviorTree.AddChildren(wanders);
    }

    private void Update() 
    { 
        if ((behaviorTreeStatus == TaskStatus.NONE) || (behaviorTreeStatus == TaskStatus.RUNNING))
        {
            behaviorTreeStatus = behaviorTree.Run(this, worldManager);
        }
    }
}