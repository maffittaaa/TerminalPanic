using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private WorldInteractions worldInteractions;
    [SerializeField] private Collider trigger;
    [SerializeField] private PlayerFainting playerFainting;
    [SerializeField] private TextMeshProUGUI wonText;

    private void Start()
    {
        wonText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && worldInteractions.gotTicket)
        {
            wonText.enabled = true;
            wonText.text = "You feel calm now...";
            playerFainting.ResetAndStartFade();
        }
    }
}
