using System.Collections.Generic;
using UnityEngine;

public enum EClothesAndAccessoriesTypes
{
    INVALID = -1,
    Jeans,
    Blouse,
    Purse,
    Hat
}

public enum EColorTypes
{
    INVALID = -1,
    Blue,
    Black,
    Green,
    Red,
    Yellow
}

public class HintFinder : MonoBehaviour
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
