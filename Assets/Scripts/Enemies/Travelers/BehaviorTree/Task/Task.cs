using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TaskStatus
{
    NONE, SUCCESS, FAILURE, RUNNING
}

public abstract class Task 
{
    protected List<Task> children;
    public TaskStatus status;

    public abstract TaskStatus Run(Agent agent, WorldManager manager);

    public Task()
    {
        children = new List<Task>();
        status = TaskStatus.NONE;
    }

    public void AddChildren(Task task)
    {
        children.Add(task);
    }
}
