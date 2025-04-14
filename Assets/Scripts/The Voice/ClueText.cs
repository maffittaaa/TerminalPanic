using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClueText : MonoBehaviour
{
    private Dictionary<EClothesAndAccessoriesTypes, EColorTypes> usedClues = new Dictionary<EClothesAndAccessoriesTypes, EColorTypes>();
    public List<EClothesAndAccessoriesTypes> clothesClues = new List<EClothesAndAccessoriesTypes>();
    public List<EColorTypes> colorClues = new List<EColorTypes>();
    [SerializeField] private IdentifyingThief thief;
    private TextMeshProUGUI clueText;
    private int numberOfClues = -1;
    private bool thiefFound = false;
    
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
        numberOfClues++;
        if (numberOfClues >= thief.thiefClothes.Count)
        {
            thiefFound = true;
            clueText.text = "Catch the Thief!";
        }
        else
        {
            if (!usedClues.ContainsKey(clothesClues[numberOfClues]))
                usedClues.Add(clothesClues[numberOfClues], colorClues[numberOfClues]);
        
            clueText.enabled = true;
            clueText.text = $"Clue {usedClues.Count - 1}: Find the person who has {clothesClues[numberOfClues]} with a {colorClues[numberOfClues]} color";
        }
        return clueText.text;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            TextForClue();
    }
}
