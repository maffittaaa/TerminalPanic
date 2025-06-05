using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClothes : MonoBehaviour
{
    [field: SerializeField] public GameObject[] travelerPossibleClothes { get; set; }
    [field: SerializeField] public ClothesSlots[] travelerClothes { get; set; }
    
}
