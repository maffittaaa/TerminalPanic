using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Task
{
    public override TaskStatus Run(Agent agent, WorldManager manager)
    {
        int failureCount = 0;
        foreach (Task task in children)
        {
            if (task.status != TaskStatus.FAILURE)
            {
                TaskStatus childrenStatus = task.Run(agent, manager);
                if (childrenStatus == TaskStatus.SUCCESS)
                {
                    status = TaskStatus.SUCCESS;
                    return status;
                }
                else if (childrenStatus == TaskStatus.FAILURE)
                {
                    failureCount++;
                }
                else if (childrenStatus == TaskStatus.RUNNING)
                {
                    status = TaskStatus.RUNNING;
                    return status;
                }
                else
                    break;
            }        
        }
        if (failureCount == children.Count)
            status = TaskStatus.FAILURE;
        else
            status = TaskStatus.RUNNING;
        
        return status;
    }
}