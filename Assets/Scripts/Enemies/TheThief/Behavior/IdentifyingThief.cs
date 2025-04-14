using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdentifyingThief : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject thief;
    [SerializeField] private ClueText clue;
    public List<EClothesAndAccessoriesTypes> thiefClothes = new List<EClothesAndAccessoriesTypes>();
    public List<EColorTypes> thiefClothesColor = new List<EColorTypes>();
    
    public HintFinder hintsForThief;
    private int peopleNeededToNextClue;
    private int numberOfObjects;
    public int clothesAndAccessoriesNumber;
    
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
            if (k >= 1000)
            {
                Debug.Log("something went wrong");
                break;
            }
            
            EClothesAndAccessoriesTypes clothesType = (EClothesAndAccessoriesTypes)Random.Range(1, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
            for (int j = 0; j < hintsForThief.clothesAndAccessoriesTypes.Count; j++)
            {
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

        for (i = 0; i < clothesAndAccessoriesNumber; i++)
        {
            thiefClothes.Add(hintsForThief.clothesAndAccessoriesTypes[i]);
            thiefClothesColor.Add(hintsForThief.colorTypes[i]);
        }
    }
    
    // public bool DoesTheThiefHasThis(int clothesAndAccessories, int color)
    // {
    //     
    // }
    //
    // public void SeeingThief()
    // {
    //     
    // }
}
