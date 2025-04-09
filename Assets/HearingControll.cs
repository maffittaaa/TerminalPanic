using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingControl : MonoBehaviour
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
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null || other.CompareTag("Player")) 
        {
            if (player != null && (player.behaviorType == BehaviorType.Walking || player.behaviorType == BehaviorType.Runing || player.behaviorType == BehaviorType.Jumping))
            {
                if (player.behaviorType != BehaviorType.Crouching)
                {
                    MyTraveller.iHearPlayer = true;
                }

                else
                {
                    MyTraveller.iHearPlayer = false; 
                }
            }

            else
            {
                MyTraveller.iHearPlayer = false; 
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null || other.CompareTag("Player"))
        {
            if (player != null && player.behaviorType == BehaviorType.Crouching)
            {
                return; 
            }

            MyTraveller.iHearPlayer = false;
        }
    }
}
