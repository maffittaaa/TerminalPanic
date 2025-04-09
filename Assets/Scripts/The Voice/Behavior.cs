using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : MonoBehaviour
{
        
    public EClothesAndAccessoriesTypes clothesAndAccessoriesTypeToFind
    {
        get; set;
    }

    public EColorTypes colorTypeToFind
    {
        get; set;
    }

    public void IdentifyingTheCorrectClues()
    {
        HintFinder[] arrayOfAllClothes = FindObjectsOfType<HintFinder>();

        for (int i = 0; i < arrayOfAllClothes.Length; ++i)
        {
            if (arrayOfAllClothes[i].clothesAndAccessoriesTypes[i] != clothesAndAccessoriesTypeToFind)
                continue;
            if (arrayOfAllClothes[i].colorTypes[i] != colorTypeToFind)
                continue;
        }
    }
}
