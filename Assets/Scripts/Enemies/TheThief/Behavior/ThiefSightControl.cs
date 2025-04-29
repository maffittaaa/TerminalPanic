using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSightControl : MonoBehaviour
{
    [field: SerializeField] public ThiefBehavior thief { get; set; }

    private void Start()
    {
        thief = transform.parent.GetComponent<ThiefBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thief.iSeePlayer = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thief.iSeePlayer = false;
        }
    }
}
