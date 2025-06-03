using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesClothes : MonoBehaviour
{
    [SerializeField] private IdentifyingThief identifyingThief;
    [SerializeField] private ChoosingClothes clothes;
    public ClothesSlots[] travelersClothes = new ClothesSlots[5];
    [SerializeField] private TravelerSpawner travelerSpawner;

    private void GivingNonEqualClothes()
    {
        bool validClothes = false;

        if (!validClothes)
        {
            clothes.ChooseRandomHeadItem();
            clothes.ChooseRandomTorsoItem();
            clothes.ChooseRandomLegsItem();
            clothes.ChooseRandomAccessoriesItem();
            
            validClothes = clothes.headPiece != identifyingThief.thiefClothes[0] &&
                           clothes.torsoPiece != identifyingThief.thiefClothes[1] &&
                           clothes.legsPiece != identifyingThief.thiefClothes[2] &&
                           clothes.accessoriesPiece != identifyingThief.thiefClothes[3];
        }

        if (validClothes)
        {
            clothes.ChooseRandomHeadItem();
            travelersClothes[0] = clothes.headPiece;
            
            clothes.ChooseRandomTorsoItem();
            travelersClothes[1] = clothes.torsoPiece;
            
            clothes.ChooseRandomLegsItem();
            travelersClothes[2] = clothes.legsPiece;
            
            clothes.ChooseRandomAccessoriesItem();
            travelersClothes[3] = clothes.accessoriesPiece;
        }

        foreach (ClothesSlots clothesItems in travelersClothes)
            clothes.InstantiatePieceOfClothing(clothesItems.model, travelerSpawner.newTraveler);
    }

    private void GiveOneIdenticalPieceOfClothing()
    {
        bool matchingWithThiefClothes = false;
        
        if (!matchingWithThiefClothes)
        {
            int randomIndex = Random.Range(0, identifyingThief.thiefClothes.Length);
            BodySlot selectedBodySlot = identifyingThief.thiefClothes[randomIndex].slot;

            foreach (BodySlot bodySlot in Enum.GetValues(typeof(BodySlot)))
            {
                if (bodySlot != selectedBodySlot)
                {
                    ClothesSlots newPieceOfClothing = null;
                    
                    if (bodySlot == BodySlot.Head)
                        newPieceOfClothing = clothes.ChooseRandomHeadItem();
                    else if (bodySlot == BodySlot.Torso)
                        newPieceOfClothing = clothes.ChooseRandomTorsoItem();
                    else if (bodySlot == BodySlot.Legs)
                        newPieceOfClothing = clothes.ChooseRandomLegsItem();
                    else if (bodySlot == BodySlot.Accessories)
                        newPieceOfClothing = clothes.ChooseRandomAccessoriesItem();
                    
                    if (newPieceOfClothing != null)
                        travelersClothes[(int)bodySlot] = newPieceOfClothing;
                }
            }
        }
        
        foreach (ClothesSlots clothesItems in travelersClothes)
            clothes.InstantiatePieceOfClothing(clothesItems.model, travelerSpawner.newTraveler);
    }

    private void GiveTwoIdenticalClothes()
    {
        bool matchingWithThiefClothes = false;
        
        if (!matchingWithThiefClothes)
        {
            List<BodySlot> selectedThiefClothes = new List<BodySlot>();
            while (selectedThiefClothes.Count < 2 && selectedThiefClothes.Count < identifyingThief.thiefClothes.Length)
            {
                int randomIndex = Random.Range(0, identifyingThief.thiefClothes.Length);
                BodySlot selectedSlot = identifyingThief.thiefClothes[randomIndex].slot;
                if (!selectedThiefClothes.Contains(selectedSlot))
                    selectedThiefClothes.Add(selectedSlot);
            }

            foreach (BodySlot bodySlot in Enum.GetValues(typeof(BodySlot)))
            {
                if (selectedThiefClothes.Contains(bodySlot))
                    continue;
                
                ClothesSlots newPieceOfClothing = null;
                
                if (bodySlot == BodySlot.Head)
                    newPieceOfClothing = clothes.ChooseRandomHeadItem();
                else if (bodySlot == BodySlot.Torso)
                    newPieceOfClothing = clothes.ChooseRandomTorsoItem();
                else if (bodySlot == BodySlot.Legs)
                    newPieceOfClothing = clothes.ChooseRandomLegsItem();
                else if (bodySlot == BodySlot.Accessories)
                    newPieceOfClothing = clothes.ChooseRandomAccessoriesItem();
                
                if (newPieceOfClothing != null)
                    travelersClothes[(int)bodySlot] = newPieceOfClothing;
            }
        }
        
        foreach (ClothesSlots clothesItems in travelersClothes)
            clothes.InstantiatePieceOfClothing(clothesItems.model, travelerSpawner.newTraveler);
    }

    public void SpawnClothes(int i)
    {
        GivingNonEqualClothes();
        /*if (i <= travelerSpawner.travelerCount * 0.2)
        {
            GivingNonEqualClothes();
            Debug.Log("FODASSE");
        }
        else if (i > travelerSpawner.travelerCount * 0.2 && i <= travelerSpawner.travelerCount * 0.6)
        {
            GiveOneIdenticalPieceOfClothing();
            Debug.Log("para esta");
        }
        else
        {
            GiveTwoIdenticalClothes();
            Debug.Log("MeRdA");
        }*/
    }
}
