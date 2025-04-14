using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClueText : MonoBehaviour
{
    private TextMeshProUGUI clueText;
    private HashSet<int> usedClues = new HashSet<int>();
    private EClothesAndAccessoriesTypes clothesAndAccessories;
    private EColorTypes color;
    public int newClue = 0;
    
    private void Start()
    {
        clueText = GetComponent<TextMeshProUGUI>();
        clueText.enabled = false;
    }

    public string TextForClue()
    {
        int safetyCounter = 5;
        int maxClothes = Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1;
        int maxColors = Enum.GetValues(typeof(EColorTypes)).Length - 1;

        while (safetyCounter > 0) //safety
        {
            clothesAndAccessories = (EClothesAndAccessoriesTypes)Random.Range(1, maxClothes); //random clothes
            color = (EColorTypes)Random.Range(1, maxColors); //random colors

            newClue = ((int)clothesAndAccessories * maxColors) + (int)color; //multiply by max colors, because it can give the same int, meaning the "same clue"
            
            if (!usedClues.Contains(newClue))
                usedClues.Add(newClue);
            
            safetyCounter--;
        }
        
        clueText.enabled = true;
        return clueText.text = $"Clue {usedClues.Count}: Find the person who has {clothesAndAccessories} with a {color} color";
    }
}
