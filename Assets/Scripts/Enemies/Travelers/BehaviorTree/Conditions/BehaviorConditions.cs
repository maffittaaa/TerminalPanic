using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType { IS_WANDER, IS_SITTER, CHECK_PLAYER_SIGHT, LOST_PLAYER }

public class BehaviorConditions : Task
{
    private ConditionType type;
    private string name;

    public BehaviorConditions(string name, ConditionType type)
    {
        this.name = name;
        this.type = type;
    }

    public override TaskStatus Run(Agent agent, WorldManager worldManager)
    {
        switch (type)
        {
            case ConditionType.CHECK_PLAYER_SIGHT:
                if (worldManager.IsPlayerInSight())
                {
                    Debug.Log("PlayerInSight");
                    status = TaskStatus.SUCCESS;
                }
                else
                {
                    Debug.Log("PlayerInSight FAILURE");
                    status = TaskStatus.FAILURE;
                }
                break;            
            case ConditionType.LOST_PLAYER:
                if (worldManager.PlayerLost())
                {
                    Debug.Log("LostPlayer");
                    status = TaskStatus.SUCCESS;
                }
                else
                {
                    Debug.Log("LostPlayer FAILURE");
                    status = TaskStatus.FAILURE;
                }
                break;
            case ConditionType.IS_WANDER:
                if (worldManager.IsWander())
                {
                    Debug.Log("WanderGuy");
                    status = TaskStatus.SUCCESS;
                }
                else
                {
                    Debug.Log("WanderGuy FAILURE");
                    status = TaskStatus.FAILURE;
                }
                break;
            case ConditionType.IS_SITTER:
                if (worldManager.IsSitter())
                {
                    Debug.Log("Sitter");
                    status = TaskStatus.SUCCESS;
                }
                else
                {
                    Debug.Log("Sitter FAILURE");
                    status = TaskStatus.FAILURE;
                }
                break;
        }

        return status;
    }
}