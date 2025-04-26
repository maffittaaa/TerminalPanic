using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightControl : MonoBehaviour
{
    [field: SerializeField] public TravelerAI traveler { get; set; }

    private void Start()
    {
        traveler = transform.parent.GetComponent<TravelerAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            traveler.iSeePlayer = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            traveler.iSeePlayer = false;
        }
        
    }
}
