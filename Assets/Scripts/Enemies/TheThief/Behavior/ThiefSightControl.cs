using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSightControl : MonoBehaviour
{
    public ThiefBehavior thief;

    private void Start()
    {
        thief = FindObjectOfType<ThiefBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
            thief.iSeePlayer = true;
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
            thief.iSeePlayer = false;
    }
}
