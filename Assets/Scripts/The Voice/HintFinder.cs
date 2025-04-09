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
    [field: SerializeField] public EClothesAndAccessoriesTypes[] clothesAndAccessoriesTypes
    {
        get;
        private set;
    }

    [field: SerializeField] public EColorTypes[] colorTypes
    {
        get;
        private set;
    }
}
