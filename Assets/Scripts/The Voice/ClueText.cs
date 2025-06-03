using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueText : MonoBehaviour
{
    private List<ClothesSlots> usedClues = new List<ClothesSlots>();
    public List<ClothesSlots> clues = new List<ClothesSlots>();
    [SerializeField] private IdentifyingThief thief;
    private TextMeshProUGUI clueText;
    public int numberOfClues = -1;
    
    private void Start()
    {
        clueText = GetComponent<TextMeshProUGUI>();
        clueText.enabled = false;

        for (int i = 0; i < thief.thiefClothes.Length; i++)
            clues.Add(thief.thiefClothes[i]);
    }

    public string TextForClue()
    {
        numberOfClues++;
        if (numberOfClues >= thief.thiefClothes.Length)
            clueText.text = "Catch the Thief!";
        else
        {
            if (!usedClues.Contains(clues[numberOfClues]))
                usedClues.Add(clues[numberOfClues]);
        
            clueText.enabled = true;
            clueText.text = $"Clue {usedClues.Count}: Find the person who has {clues[numberOfClues]}";
        }
        return clueText.text;
    }
}
