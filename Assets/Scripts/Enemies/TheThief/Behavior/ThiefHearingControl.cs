using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefHearingControl : MonoBehaviour
{
    [field: SerializeField] public ThiefBehavior thief { get; set; }

    private void Start()
    {
        thief = transform.parent.GetComponent<ThiefBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.transform.parent.GetComponent<PlayerMovement>();

            if (player != null)
            {   
                if (player.behaviorType != BehaviorType.Crouching && player.behaviorType != BehaviorType.Idleing)
                    thief.iHearPlayer = true;
                else
                    thief.iHearPlayer = false;
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
                    thief.iHearPlayer = true;
                else
                    thief.iHearPlayer = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            thief.iHearPlayer = false;
    }
}
