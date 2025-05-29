using System.Collections.Generic;
using UnityEngine;

public enum ETopClothesType
{
    None,
    LongSleeves,
    TShirt,
    Top
}

public enum EBottomClothesType
{
    None,
    Jeans,
    Skirt,
    Shorts
}

public enum EAccessoriesType
{
    None,
    Hat,
    Headphones
}

public enum EPursesType
{
    None,
    PurseAndTrolley,
    Backpack,
    Trolley
}

public class ClothesAndColors : MonoBehaviour
{
    [field: SerializeField] public List<ETopClothesType> topClothesTypes
    {
        get;
        set;
    }
    
    [field: SerializeField] public List<EBottomClothesType> bottomClothesTypes
    {
        get;
        set;
    }
    
    [field: SerializeField] public List<EAccessoriesType> accessoriesTypes
    {
        get;
        set;
    }
    
    [field: SerializeField] public List<EPursesType> pursesTypes
    {
        get;
        set;
    }
}
