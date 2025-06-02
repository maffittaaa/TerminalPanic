using System.Collections.Generic;
using UnityEngine;

public class EnemiesClothes : MonoBehaviour
{
    [SerializeField] private IdentifyingThief identifyingThief;
    private ChoosingClothes clothes;
    public List<ClothesSlots> travelersClothes = new List<ClothesSlots>();
    private TravelerSpawner travelerSpawner;

    private void GivingNonEqualClothes()
    {
        int maxAttempts = 100;
        int attempts = 0;
        bool validClothes = false;

        while (attempts < maxAttempts && !validClothes)
        {
            travelersClothes.Clear();
            clothes.HeadSlot();
            clothes.TorsoSlot();
            clothes.LegsSlot();
            clothes.AccessoriesSlot();

            validClothes = clothes.headPiece != identifyingThief.thiefClothes[0] &&
                           clothes.torsoPiece != identifyingThief.thiefClothes[1]
                           && clothes.legsPiece != identifyingThief.thiefClothes[2] &&
                           clothes.accessoriesPiece != identifyingThief.thiefClothes[3];
            attempts++;
        }

        if (validClothes)
        {
            travelersClothes.Add(clothes.headPiece);
            travelersClothes.Add(clothes.torsoPiece);
            travelersClothes.Add(clothes.legsPiece);
            travelersClothes.Add(clothes.accessoriesPiece);
        }
    }

    private void GivingOneIdenticalPieceOfClothing()
    {
        int maxAttempts = 100;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            travelersClothes.Clear();
            
        }

        
    }

    private void GivingTwoIdenticalPieceOfClothing()
    {
        
    }

    public void SpawningClothes(int i)
    {
        if (i <= travelerSpawner.travelerCount * 0.2)
            GivingNonEqualClothes();
        else if (i > travelerSpawner.travelerCount * 0.2 && i <= travelerSpawner.travelerCount * 0.6)
            GivingOneIdenticalPieceOfClothing();
        else
            GivingTwoIdenticalPieceOfClothing();
    }
}
