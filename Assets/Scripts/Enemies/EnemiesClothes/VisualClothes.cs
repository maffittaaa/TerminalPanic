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
    private TextMeshPro clothesText;
    private TextMeshPro colorText;
    private List<EClothesAndAccessoriesTypes> clothesList = new List<EClothesAndAccessoriesTypes>();
    private List<EColorTypes> colorList = new List<EColorTypes>();
    private PlayerMovement player;

    private void Start()
    {
        traveler = gameObject;
        player = FindObjectOfType<PlayerMovement>();
        
        GameObject clothes = new GameObject("ClothesText");
        clothes.transform.SetParent(traveler.transform);
        clothesText = clothes.AddComponent<TextMeshPro>();
        clothesText.fontSize = 5;
        clothes.transform.localPosition = new Vector3(10.9f, 0.5f, 0);
        
        GameObject colors = new GameObject("ColorsText");
        colors.transform.SetParent(traveler.transform);
        colorText = colors.AddComponent<TextMeshPro>();
        colorText.fontSize = 5;
        colors.transform.localPosition = new Vector3(10.9f, 0f, 0);
        
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
            clothesText.text += $"\n {clothesList[j]}\n";
            colorText.text += $"\n {colorList[j]}\n";
        }
    }
}
