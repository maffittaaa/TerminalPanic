using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesClothes : MonoBehaviour
{
    public List<EClothesAndAccessoriesTypes> clothesForEnemies = new List<EClothesAndAccessoriesTypes>();
    public List<EColorTypes> colorForClothes = new List<EColorTypes>();
    [SerializeField] private IdentifyingThief identifyingThief;
    public ClothesAndColors clothesAndColors;
    public int clothesNumber;

    public void GivingClothesToEnemies()
    {
        clothesForEnemies.Clear();
        colorForClothes.Clear();
        
        EClothesAndAccessoriesTypes clothes = (EClothesAndAccessoriesTypes)Random.Range(0, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
        EColorTypes color = (EColorTypes)Random.Range(0, Enum.GetValues(typeof(EColorTypes)).Length - 1);
        
        int k = 0;
        clothesNumber = Random.Range(1, 3);
        int j = 0;
        while (j < clothesNumber)
        {
            bool equalClothes = false;

            if (k >= 1000)
            {
                Debug.Log("something went wrong");
                break;
            }

            bool thiefsClothes = identifyingThief.hintsForThief.clothesAndAccessoriesTypes.Contains(clothes);

            if (!thiefsClothes && !clothesForEnemies.Contains(clothes))
            {
                clothesForEnemies.Add(clothes);
                colorForClothes.Add(color);
                j++;
            }
            else
            {
                equalClothes = true;
                clothes = (EClothesAndAccessoriesTypes)Random.Range(0, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
            }
            k++;
        }

        for (int i = 0; i < clothesForEnemies.Count; i++)
        {
            clothesAndColors.clothesAndAccessoriesTypes.Add(clothesForEnemies[i]);
            clothesAndColors.colorTypes.Add(colorForClothes[i]);
        }
    }

    public void MatchingWithOneCloth()
    {
        clothesForEnemies.Clear();
        colorForClothes.Clear();
        
        List<EClothesAndAccessoriesTypes> thiefClothes = identifyingThief.hintsForThief.clothesAndAccessoriesTypes;
        List<EColorTypes> thiefColorOfClothes = identifyingThief.hintsForThief.colorTypes;
        int k = 0;
        
        clothesNumber = Random.Range(1, 3);
        bool matchingWithThiefClothes = false;
        
        int j = 0;
        while (j < clothesNumber)
        {
            if (k >= 100)
            {
                Debug.Log("something went wrong");
                break;
            }

            EClothesAndAccessoriesTypes clothes;
            EColorTypes color;
            bool matching = false;

            if (!matchingWithThiefClothes)
            {
                int randomCloth = Random.Range(0, thiefClothes.Count - 1);
                int randomColor = Random.Range(0, thiefColorOfClothes.Count - 1);
                clothes = thiefClothes[randomCloth];
                color = thiefColorOfClothes[randomColor];
                matching = true;
            }
            else
            {
                while (true)
                {
                    clothes = (EClothesAndAccessoriesTypes)Random.Range(0, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
                    color = (EColorTypes)Random.Range(0, Enum.GetValues(typeof(EColorTypes)).Length - 1);

                    if (!thiefClothes.Contains(clothes) && !clothesForEnemies.Contains(clothes) && 
                        !thiefColorOfClothes.Contains(color) && !colorForClothes.Contains(color))
                        break;
                }
            }
            if (!clothesForEnemies.Contains(clothes) || matching)
            {
                clothesForEnemies.Add(clothes);
                colorForClothes.Add(color);
                matchingWithThiefClothes = true;
                j++;
            }
            k++;
        }
        
        for (int i = 0; i < clothesForEnemies.Count; i++)
        {
            clothesAndColors.clothesAndAccessoriesTypes.Add(clothesForEnemies[i]);
            clothesAndColors.colorTypes.Add(colorForClothes[i]);
        }
    }

    public void MatchingWithTwoClothes()
    {
        clothesForEnemies.Clear();
        colorForClothes.Clear();
        
        List<EClothesAndAccessoriesTypes> thiefClothes = identifyingThief.hintsForThief.clothesAndAccessoriesTypes;
        List<EColorTypes> thiefColorOfClothes = identifyingThief.hintsForThief.colorTypes;
        List<EClothesAndAccessoriesTypes> clothesUsed = new List<EClothesAndAccessoriesTypes>();
        List<EColorTypes> colorsUsed = new List<EColorTypes>();
        int k = 0;
        
        clothesNumber = Random.Range(1, 3);
        int matchingWithThiefClothes = 0;
        
        int j = 0;
        while (j < clothesNumber)
        {
            if (k >= 10000)
            {
                Debug.Log("something went wrong");
                break;
            }

            EClothesAndAccessoriesTypes clothes;
            EColorTypes color;
            bool matching = false;

            if (matchingWithThiefClothes < 2)
            {
                int randomCloth = Random.Range(0, thiefClothes.Count - 1);
                int randomColor = Random.Range(0, thiefColorOfClothes.Count - 1);
                clothes = thiefClothes[randomCloth];
                color = thiefColorOfClothes[randomColor];
                matching = true;
            }
            else
            {
                while (true)
                {
                    clothes = (EClothesAndAccessoriesTypes)Random.Range(0, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
                    color = (EColorTypes)Random.Range(0, Enum.GetValues(typeof(EColorTypes)).Length - 1);
                    if (!thiefClothes.Contains(clothes) && !clothesForEnemies.Contains(clothes) &&
                        !thiefColorOfClothes.Contains(color) && !colorForClothes.Contains(color) 
                        && !clothesUsed.Contains(clothes) && !colorsUsed.Contains(color))
                        break;
                }
            }

            if (!clothesUsed.Contains(clothes) && !colorsUsed.Contains(color))
            {
                clothesForEnemies.Add(clothes);
                colorForClothes.Add(color);
                clothesUsed.Add(clothes);
                colorsUsed.Add(color);
                if (matching)
                    matchingWithThiefClothes++;
                j++;
            }
            k++;
        }
        
        for (int i = 0; i < clothesForEnemies.Count; i++)
        {
            clothesAndColors.clothesAndAccessoriesTypes.Add(clothesForEnemies[i]);
            clothesAndColors.colorTypes.Add(colorForClothes[i]);
        }
    }
}
