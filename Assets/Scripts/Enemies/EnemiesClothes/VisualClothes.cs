using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualClothes : MonoBehaviour
{
    [field: Header("Traveler")]
    private GameObject traveler;
    public TravelerSpawner travelerSpawner;
    
    [field: Header("Travelers Clothes")]
    private TextMeshPro clothesAndColorsText;
    private List<EClothesAndAccessoriesTypes> clothesList = new List<EClothesAndAccessoriesTypes>();
    private List<EColorTypes> colorList = new List<EColorTypes>();
    
    private void Start()
    {
        traveler = gameObject;
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
