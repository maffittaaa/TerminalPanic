using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingControl : MonoBehaviour
{

    public ThiefBehavior enemy;

    private void Start()
    {
        enemy = FindObjectOfType<ThiefBehavior>();
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
                    enemy.iHearPlayer = true;
                    Debug.Log("Player is moving and making noise. iHearPlayer = true");
                }
                else
                {
                    enemy.iHearPlayer = false; 
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
                enemy.iHearPlayer = true;
                Debug.Log("Player is making noise. iHearPlayer = true (OnTriggerStay)");
            }
            else
            {
                enemy.iHearPlayer = false;
                Debug.Log("Player is quiet or crouching. iHearPlayer = false (OnTriggerStay)");
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.iHearPlayer = false;
            Debug.Log("Player exited hearing zone. iHearPlayer = false");
        }
    }


}
