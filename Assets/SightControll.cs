using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightControl : MonoBehaviour
{

    public ThiefBehavior enemy;

    private void Start()
    {
        enemy = FindObjectOfType<ThiefBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            enemy.iSeePlayer = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            enemy.iSeePlayer = false;
        }
        
    }
}
