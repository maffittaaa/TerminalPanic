using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CluesTrigger : MonoBehaviour
{
    [SerializeField] private ClueText clueText;
    [SerializeField] private WorldInteractions interactions;
    [field: SerializeField] public Collider trigger { get; set; }

    private void Start()
    {
        trigger.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactions.body.activeSelf)
        {
            clueText.TextForClue();
            trigger.isTrigger = false;
        }
    }
}
