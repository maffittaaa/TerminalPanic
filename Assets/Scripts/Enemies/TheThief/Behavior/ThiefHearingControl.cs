using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefHearingControl : MonoBehaviour
{
    public ThiefBehavior thief;

    private void Start()
    {
        thief = FindObjectOfType<ThiefBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null || other.CompareTag("Player"))
        {
            if (player != null)
            {   
                if (player.behaviorType != BehaviorType.Crouching && (player.behaviorType == BehaviorType.Walking || player.behaviorType == BehaviorType.Runing || player.behaviorType == BehaviorType.Jumping))
                    thief.iHearPlayer = true;
                else
                    thief.iHearPlayer = false; 
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            if (player.behaviorType != BehaviorType.Crouching && (player.behaviorType == BehaviorType.Walking || player.behaviorType == BehaviorType.Runing || player.behaviorType == BehaviorType.Jumping))
                thief.iHearPlayer = true;
            else
                thief.iHearPlayer = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            thief.iHearPlayer = false;
    }
}
