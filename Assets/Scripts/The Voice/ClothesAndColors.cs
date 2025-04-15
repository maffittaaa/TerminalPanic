using System.Collections.Generic;
using UnityEngine;

public enum EClothesAndAccessoriesTypes
{
    INVALID = -1,
    Jeans,
    Blouse,
    Purse,
    Hat,
    Shoes,
    Belt,
    Necklace,
    Bracelet,
    Scarf
}

public enum EColorTypes
{
    INVALID = -1,
    Blue,
    Black,
    Green,
    Red,
    Yellow,
    Pink,
    Orange,
    Purple
}

public class ClothesAndColors : MonoBehaviour
{
    [field: SerializeField] public List<EClothesAndAccessoriesTypes> clothesAndAccessoriesTypes
    {
        get;
        set;
    }

    [field: SerializeField] public List<EColorTypes> colorTypes
    {
        get;
        set;
    }
}
