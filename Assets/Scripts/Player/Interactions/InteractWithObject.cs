using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractWithObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemType itemType;

    public virtual void Interact(GameObject instigator)
    {
        
    }
}
