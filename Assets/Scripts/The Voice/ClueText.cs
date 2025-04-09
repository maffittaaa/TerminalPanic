using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClueText : MonoBehaviour
{
    private TextMeshProUGUI clueText;
    
    private void Start()
    {
        clueText = GetComponent<TextMeshProUGUI>();
        clueText.enabled = false;
    }

    public string TextForClue()
    {
        EClothesAndAccessoriesTypes clothesAndAccessoriesTypes = (EClothesAndAccessoriesTypes)Random.Range(0, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length);
        EColorTypes colorTypes = (EColorTypes)Random.Range(0, Enum.GetValues(typeof(EColorTypes)).Length);
        
        clueText.enabled = true;
        return clueText.text = $"Clue 1: Find the person who has {clothesAndAccessoriesTypes} with a {colorTypes} color";
    }
}
