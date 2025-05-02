using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private ItemType type;

    [SerializeField] private TextMeshPro interactingText;
    [SerializeField] private Outline outline;

    public ItemType GetItemType()
    {
        return type;
    }

    private void Start()
    {
        outline = GetComponent<Outline>();
        interactingText = GameObject.FindGameObjectWithTag("InteractText").GetComponent<TextMeshPro>();
    }

    public void ChangeOutlineState(bool state)
    {
        outline.enabled = state;
        interactingText.enabled = state;
    }

    private void OnDestroy()
    {
        if(interactingText)
            interactingText.enabled = false;
    }
}