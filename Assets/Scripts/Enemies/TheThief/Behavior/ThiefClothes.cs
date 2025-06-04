using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefClothes : MonoBehaviour
{
    [field: Header("Clothes")]
    private ChoosingClothes clothes;
    public ClothesSlots[] thiefClothes;
    [SerializeField] private GameObject[] clothesModels;
    
    private void Awake()
    {
        thiefClothes = new ClothesSlots[Enum.GetValues(typeof(BodySlot)).Length];
        clothes = GameObject.FindGameObjectWithTag("ClothesManager").GetComponent<ChoosingClothes>();
        
        clothes.ChooseRandomHeadItem();
        thiefClothes[0] = clothes.headPiece;
        
        clothes.ChooseRandomTorsoItem();
        thiefClothes[1] = clothes.torsoPiece;
        
        clothes.ChooseRandomLegsItem();
        thiefClothes[2] = clothes.legsPiece;
        
        clothes.ChooseRandomAccessoriesItem();
        thiefClothes[3] = clothes.accessoriesPiece;
        
        for (int i = 0; i < thiefClothes.Length; i++)
        {
            for (int j = 0; j < clothesModels.Length; j++)
            {
                if (thiefClothes[i] != null && clothesModels[j] != null && thiefClothes[i].id == j)
                {
                    clothesModels[j].SetActive(true);
                    thiefClothes[i].model = clothesModels[j];
                }
            }
        }
    }
}
