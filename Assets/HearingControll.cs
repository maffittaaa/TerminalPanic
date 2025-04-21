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

            Debug.Log("Player entered the hearing zone");


            if (player != null)
            {   
                if (player.behaviorType != BehaviorType.Crouching &&
                    (player.behaviorType == BehaviorType.Walking || player.behaviorType == BehaviorType.Runing || player.behaviorType == BehaviorType.Jumping))

                {
                    MyTraveller.iHearPlayer = true;
                    Debug.Log("Player is moving and making noise. iHearPlayer = true");
                }
                else
                {
                    MyTraveller.iHearPlayer = false; 
                    Debug.Log("Player is crouching or idle. iHearPlayer = false");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            if (player.behaviorType != BehaviorType.Crouching &&
                (player.behaviorType == BehaviorType.Walking ||
                 player.behaviorType == BehaviorType.Runing ||
                 player.behaviorType == BehaviorType.Jumping))
            {
                MyTraveller.iHearPlayer = true;
                Debug.Log("Player is making noise. iHearPlayer = true (OnTriggerStay)");
            }
            else
            {
                MyTraveller.iHearPlayer = false;
                Debug.Log("Player is quiet or crouching. iHearPlayer = false (OnTriggerStay)");
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null || other.CompareTag("Player"))
        {

            Debug.Log("Player exited the hearing zone");

            if (player != null && player.behaviorType != BehaviorType.Crouching && (player.behaviorType == BehaviorType.Idleing || player.behaviorType == BehaviorType.Crouching))
            {
                MyTraveller.iHearPlayer = false;
                Debug.Log("Player stopped moving or crouching, iHearPlayer = false");
            }
        }
    }


}
