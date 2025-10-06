using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueText : MonoBehaviour
{
    public List<ClothesSlots> usedClues = new List<ClothesSlots>();
    public List<ClothesSlots> clues = new List<ClothesSlots>();
    private ThiefClothes thief;
    [SerializeField] private TheVoiceAudio voice;
    private TextMeshProUGUI clueText;
    public int numberOfClues = 0;
    [SerializeField] private IdentifyingThief identifyingThief;
    
    private void Start()
    {
        thief = (ThiefClothes)FindObjectOfType(typeof(ThiefClothes));
        clueText = GetComponent<TextMeshProUGUI>();
        clueText.enabled = false;
        numberOfClues = 0;
        for (int i = 0; i < thief.thiefClothes.Length; i++)
            clues.Add(thief.thiefClothes[i]);
    }

    public string TextForClue()
    {
        if (numberOfClues >= thief.thiefClothes.Length)
            clueText.text = "Catch the Thief!";
        else
        {
            if (!usedClues.Contains(clues[numberOfClues]) && clues[numberOfClues] != null)
                usedClues.Add(clues[numberOfClues]);

            if (clues[numberOfClues] == null)
            {
                numberOfClues++;
                TextForClue();
            }
            else
            {
                clueText.enabled = true;
                clueText.text = $"Clue {usedClues.Count}: Find the person who has {clues[numberOfClues].name}! ({identifyingThief.peopleCountToNextClue} / {identifyingThief.peopleNeededToNextClue} left)";
                voice.PlayAudio(thief.thiefClothes[numberOfClues].id);
            }
        }
        return clueText.text;
    }
}
