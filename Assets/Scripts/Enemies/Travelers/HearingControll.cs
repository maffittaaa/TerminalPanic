using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingControl : MonoBehaviour
{
    [field: SerializeField] public TravelerAI traveler { get; set; }

    private void Start()
    {
        traveler = transform.parent.GetComponent<TravelerAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.transform.parent.GetComponent<PlayerMovement>();

            Debug.Log("Player entered the hearing zone");

            if (player != null)
            {   
                if (player.behaviorType != BehaviorType.Crouching && player.behaviorType != BehaviorType.Idleing)
                {
                    traveler.iHearPlayer = true;
                    Debug.Log("Player is moving and making noise. iHearPlayer = true");
                }
                else
                {
                    traveler.iHearPlayer = false; 
                    Debug.Log("Player is crouching or idle. iHearPlayer = false");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.transform.parent.GetComponent<PlayerMovement>();

            if (player != null)
            {
                if (player.behaviorType != BehaviorType.Crouching && player.behaviorType != BehaviorType.Idleing)
                {
                    traveler.iHearPlayer = true;
                    Debug.Log("Player is moving and making noise. iHearPlayer = true");
                }
                else
                {
                    traveler.iHearPlayer = false;
                    Debug.Log("Player is crouching or idle. iHearPlayer = false");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            traveler.iHearPlayer = false;
            Debug.Log("Player exited hearing zone. iHearPlayer = false");
        }
    }


}
