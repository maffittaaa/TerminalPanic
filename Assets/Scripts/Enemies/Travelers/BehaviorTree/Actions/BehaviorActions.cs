using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { WANDER, STAY_STILL, CHASE, SUSPICIOUS, RETURN }

public class BehaviorActions : Task
{
    private ActionType type;
    private string name;

    public BehaviorActions(string name, ActionType type)
    {
        this.name = name;
        this.type = type;
    }
    
    public override TaskStatus Run(Agent agent, WorldManager worldManager)
    {
        switch (type)
        {
            case ActionType.WANDER:
                worldManager.Wander();
                break;
            case ActionType.STAY_STILL:
                worldManager.Wait();
                break;
            case ActionType.CHASE:
                worldManager.ChasePlayer();
                break;
            case ActionType.SUSPICIOUS:
                worldManager.Suspect();
                break;
            case ActionType.RETURN:
                worldManager.Return();
                break;
        }

        status = TaskStatus.SUCCESS;
        return status;
    }
}