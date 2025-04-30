using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VisualClothes : MonoBehaviour
{
    private GameObject traveler;
    public TravelerSpawner travelerSpawner;
    private TextMeshPro clothesAndColorsText;
    private List<EClothesAndAccessoriesTypes> clothesList = new List<EClothesAndAccessoriesTypes>();
    private List<EColorTypes> colorList = new List<EColorTypes>();
    private PlayerMovement player;

    private void Start()
    {
        traveler = gameObject;
        player = FindObjectOfType<PlayerMovement>();
        clothesAndColorsText = traveler.GetComponentInChildren<TextMeshPro>();
        TextForClothesAndColors();
    }
    
    public void TextForClothesAndColors()
    {
        clothesList.Clear();
        colorList.Clear();
        
        int colorAndAccessoriesCount = traveler.GetComponent<ClothesAndColors>().clothesAndAccessoriesTypes.Count;
        for (int j = 0; j < travelerSpawner.enemiesClothes.clothesForEnemies.Count && j < colorAndAccessoriesCount; j++)
        {
            clothesList.Add(GetComponent<ClothesAndColors>().clothesAndAccessoriesTypes[j]);
            colorList.Add(GetComponent<ClothesAndColors>().colorTypes[j]);
            clothesAndColorsText.text += $"{clothesList[j]} | {colorList[j]}\n";
        }
    }
}
