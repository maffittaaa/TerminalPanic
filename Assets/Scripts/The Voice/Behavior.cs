using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Behavior : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private ClueText clue;
    [SerializeField] private GameObject thief;
    private Dictionary<EClothesAndAccessoriesTypes, EColorTypes> howManyHasPlayerSeen = new Dictionary<EClothesAndAccessoriesTypes, EColorTypes>();
    private HintFinder hintsForThief;
    private int peopleNeededToNextClue;
    private int numberOfObjects;
    private int clothesAndAccessoriesNumber;
    
    private void Awake()
    {
        thief = Instantiate(thief, new Vector3(0, 0, 0), Quaternion.identity); //instantiate the thief into the world
        hintsForThief = thief.GetComponent<HintFinder>(); //add the hint finder component
        
        clothesAndAccessoriesNumber = Random.Range(3, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
        int i = 0;
        int k = 0;
        
        while (i < clothesAndAccessoriesNumber)
        {
            bool equalType = false;
            Debug.Log("i: " + i);
            Debug.Log("clothes: " + clothesAndAccessoriesNumber);
            if (k >= 1000)
            {
                Debug.Log("something went wrong");
                break;
            }
            
            EClothesAndAccessoriesTypes clothesType = (EClothesAndAccessoriesTypes)Random.Range(1, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
            for (int j = 0; j < hintsForThief.clothesAndAccessoriesTypes.Count; j++)
            {
                Debug.Log("j: " + j);
                Debug.Log("types of clothes number: " + hintsForThief.clothesAndAccessoriesTypes.Count);
                if (clothesType == hintsForThief.clothesAndAccessoriesTypes[j])
                {
                    equalType = true;
                    break;
                }
            }

            if (!equalType)
            {
                EColorTypes colorsType = (EColorTypes)Random.Range(1, Enum.GetValues(typeof(EColorTypes)).Length - 1);
                hintsForThief.clothesAndAccessoriesTypes.Add(clothesType);
                hintsForThief.colorTypes.Add(colorsType);
                i++;
            }
            k++;
        }
    }
    
    // public void IdentifyingTheCorrectClues(int clothesAndAccessories, int color)
    // {
    //     EClothesAndAccessoriesTypes clueType1 = hintsForThief.clothesAndAccessoriesTypes[clothesAndAccessories];
    //     
    //    if (!howManyHasPlayerSeen.ContainsKey(clueType1))
    //      howManyHasPlayerSeen[clueType1] = 0;
    //
    //     howManyHasPlayerSeen[clueType1]++;
    //         
    //     if (howManyHasPlayerSeen[clueType1] == peopleNeededToNextClue.ConvertTo<EColorTypes>()) //this is wrong!!!!!!
    //        clue.TextForClue();
    // }
}
