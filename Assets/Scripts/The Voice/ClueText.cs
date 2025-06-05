using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueText : MonoBehaviour
{
    public List<ClothesSlots> usedClues = new List<ClothesSlots>();
    public List<ClothesSlots> clues = new List<ClothesSlots>();
    private ThiefClothes thief;
    private TextMeshProUGUI clueText;
    public int numberOfClues = -1;
    [SerializeField] private IdentifyingThief identifyingThief;
    
    private void Start()
    {
        thief = (ThiefClothes)FindObjectOfType(typeof(ThiefClothes));
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

            if (clues[numberOfClues] == null)
                numberOfClues++;
            else
            {
                clueText.enabled = true;
                clueText.text = $"Clue {usedClues.Count}: Find the person who has {clues[numberOfClues].name}! ({identifyingThief.peopleNeededToNextClue} left)";
            }
        }
        return clueText.text;
    }
}
