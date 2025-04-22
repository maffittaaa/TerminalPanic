using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightControl : MonoBehaviour
{

    public TravelerAI Enemy;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            Enemy.iSeePlayer = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            Enemy.iSeePlayer = false;
        }
        
    }
}
