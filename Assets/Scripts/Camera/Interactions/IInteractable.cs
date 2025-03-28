using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public virtual void Interact(GameObject instigator)
    {
        Debug.Log("Interactable Interface Not Implemented");
    }
}
