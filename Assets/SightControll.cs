using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightControl : MonoBehaviour
{

    public TravelerAI MyTraveller;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            MyTraveller.iSeePlayer = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<PlayerMovement>() || other.tag == "Player")
        {
            MyTraveller.iSeePlayer = false;
        }
        
    }
}
