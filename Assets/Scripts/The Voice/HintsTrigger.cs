using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HintsTrigger : MonoBehaviour
{
    [SerializeField] private ClueText clueText;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            clueText.TextForClue();
    }
}
