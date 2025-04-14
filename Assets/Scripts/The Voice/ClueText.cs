using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClueText : MonoBehaviour
{
    private TextMeshProUGUI clueText;
    private Dictionary<EClothesAndAccessoriesTypes, EColorTypes> usedClues = new Dictionary<EClothesAndAccessoriesTypes, EColorTypes>();
    private List<EClothesAndAccessoriesTypes> clothesClues = new List<EClothesAndAccessoriesTypes>();
    private List<EColorTypes> colorClues = new List<EColorTypes>();
    [SerializeField] private IdentifyingThief thief;
    private int numberOfClues = 0;
    
    private void Start()
    {
        clueText = GetComponent<TextMeshProUGUI>();
        clueText.enabled = false;

        for (int i = 0; i < thief.thiefClothes.Count; i++)
        {
            clothesClues.Add(thief.thiefClothes[i]);
            colorClues.Add(thief.thiefClothesColor[i]);
        }
    }

    public string TextForClue()
    {
        if (!usedClues.ContainsKey(clothesClues[numberOfClues]))
            usedClues.Add(clothesClues[numberOfClues], colorClues[numberOfClues]);
        
        clueText.enabled = true;
        clueText.text = $"Clue {usedClues.Count - 1}: Find the person who has {clothesClues[numberOfClues]} with a {colorClues[numberOfClues]} color";
        numberOfClues++;
        return clueText.text;
    }
}
