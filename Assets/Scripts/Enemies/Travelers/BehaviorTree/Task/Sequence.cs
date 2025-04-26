using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task
{
    public override TaskStatus Run(Agent agent, WorldManager manager)
    {
        int successCount = 0;
        foreach(Task task in children)
        {
            if(task.status != TaskStatus.SUCCESS)
            {
                TaskStatus childrenStatus = task.Run(agent, manager);
                if(childrenStatus == TaskStatus.FAILURE)
                {
                    Debug.Log("Failure S: " + task);
                    status = TaskStatus.FAILURE;
                    return status;
                }
                else if (childrenStatus == TaskStatus.SUCCESS)
                {
                    Debug.Log("Sucess S: " + task);
                    successCount++;
                }
                else if (childrenStatus == TaskStatus.RUNNING)
                {
                    Debug.Log("Running S: " + task);
                    status = TaskStatus.RUNNING;
                    return status;
                }
                else
                {
                    break;
                }
            }
            else
            {
                successCount++;
            }
        }
        if (successCount == children.Count)
            status = TaskStatus.SUCCESS;
        else
            status = TaskStatus.RUNNING;

        return status;
    }
}

