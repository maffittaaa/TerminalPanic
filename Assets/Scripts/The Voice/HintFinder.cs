using UnityEngine;

public enum EClothesAndAccessoriesTypes
{
    Jeans,
    Blouse,
    Purse,
    Hat
}

public enum EColorTypes
{
    Blue,
    Black,
    Green,
    Red,
    Yellow
}

public class HintFinder : MonoBehaviour
{
    [field: SerializeField] public EClothesAndAccessoriesTypes clothesAndAccessoriesTypes
    {
        get;
        set;
    }

    [field: SerializeField] public EColorTypes colorTypes
    {
        get;
        set;
    }
}
