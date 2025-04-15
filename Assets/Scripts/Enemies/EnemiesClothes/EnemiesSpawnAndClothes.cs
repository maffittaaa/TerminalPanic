using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesSpawnAndClothes : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private GameObject enemy;
    [SerializeField] private List<Vector3> spawnLocations;
    private List<EClothesAndAccessoriesTypes> clothesForEnemies = new List<EClothesAndAccessoriesTypes>();
    private List<EColorTypes> colorForClothes = new List<EColorTypes>();
    [SerializeField] private IdentifyingThief identifyingThief;
    private int maxNumberOfPeople = 10;
    private ClothesAndColors clothesAndColors;
    private int clothesNumber;

    private void Start()
    {
        for (int i = 1; i < maxNumberOfPeople; i++)
        {
            Vector3 spawnLocation = new Vector3(Random.Range(-10, -130), 19, Random.Range(10, 60));;
            
            GameObject tempEnemy = Instantiate(enemy, spawnLocation, Quaternion.identity);
            clothesAndColors = tempEnemy.GetComponent<ClothesAndColors>();
            enemies.Add(tempEnemy);
            
            if (i <= 2)
                GivingClothesTo2Enemies();
            else if (i > 2 && i <= 4)
                MatchingWithOneCloth();
            else
                MatchingWithTwoClothes();
        }
    }

    private void GivingClothesTo2Enemies()
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

    private void MatchingWithOneCloth()
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
            if (k >= 1000)
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

                    if (!thiefClothes.Contains(clothes) && !clothesForEnemies.Contains(clothes) && !thiefColorOfClothes.Contains(color) && !colorForClothes.Contains(color))
                        break;
                }
            }
            if (!clothesForEnemies.Contains(clothes) && colorForClothes.Contains(color)|| matching)
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

    private void MatchingWithTwoClothes()
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
            if (k >= 1000)
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
                    if (!thiefClothes.Contains(clothes) && !clothesForEnemies.Contains(clothes) && !thiefColorOfClothes.Contains(color) && !colorForClothes.Contains(color) && !clothesUsed.Contains(clothes) && !colorsUsed.Contains(color))
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
