using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour, IInteractable
{

    public virtual void Interact(GameObject instigator)
    {
        Debug.Log($"{instigator} is interacting with {gameObject}");
    }
}
