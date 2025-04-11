using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClueText : MonoBehaviour
{
    private TextMeshProUGUI clueText;
    private HashSet<string> usedClues = new HashSet<string>();
    
    private void Start()
    {
        clueText = GetComponent<TextMeshProUGUI>();
        clueText.enabled = false;
    }

    public string TextForClue()
    {
        string newClue = "";
        int safetyCounter = 6;

        while (safetyCounter > 0) //safety
        {
            EClothesAndAccessoriesTypes clothesAndAccessoriesTypes = (EClothesAndAccessoriesTypes)Random.Range(0, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length);
            EColorTypes colorTypes = (EColorTypes)Random.Range(0, Enum.GetValues(typeof(EColorTypes)).Length);
        
            newClue = $"{clothesAndAccessoriesTypes}-{colorTypes}"; //creates a single string that combines two enum values separating them by a -
            
            if (usedClues.Contains(newClue))
                usedClues.Add(newClue);
            
            safetyCounter--;
        }
        
        clueText.enabled = true;
        return clueText.text = $"Clue {usedClues.Count}: Find the person who has {newClue.Replace("-", " with a ")} color";
    }
}
