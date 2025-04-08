using UnityEngine;

public class HintsTrigger : MonoBehaviour
{
    public EClothesAndAccessoriesTypes clothesAndAccessoriesTypeToFind
    {
        get; set;
    }

    public EColorTypes colorTypeToFind
    {
        get; set;
    }

    private void IdentifyingTheCorrectClues()
    {
        HintFinder[] arrayOfAllClothes = FindObjectsOfType<HintFinder>();

        for (int i = 0; i < arrayOfAllClothes.Length; ++i)
        {
            if (arrayOfAllClothes[i].clothesAndAccessoriesTypes != clothesAndAccessoriesTypeToFind)
                continue;
        }
    }
    
    
    
}
