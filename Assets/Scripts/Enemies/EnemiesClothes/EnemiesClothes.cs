using UnityEngine;

public class EnemiesClothes : MonoBehaviour
{
    [SerializeField] private IdentifyingThief identifyingThief;
    private ChoosingClothes clothes;
    private ClothesSlots clothesForTravelers;

    private void GivingHeadItems()
    {
        clothes.HeadSlot();
        
    }
}
