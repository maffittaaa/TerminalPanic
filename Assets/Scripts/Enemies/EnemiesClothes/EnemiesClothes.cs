using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesClothes : MonoBehaviour
{
    [SerializeField] private IdentifyingThief identifyingThief;
    [SerializeField] private ChoosingClothes clothes;
    public ClothesSlots[] travellerClothes;
    [SerializeField] private TravelerSpawner travelerSpawner;

    private void GivingNonEqualClothes()
    {
        bool validClothes = false;
        travellerClothes = new ClothesSlots[5];
        
        while (!validClothes)
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
            travellerClothes[0] = clothes.headPiece;
            travellerClothes[1] = clothes.torsoPiece;
            travellerClothes[2] = clothes.legsPiece;
            travellerClothes[3] = clothes.accessoriesPiece;
        }

        foreach (ClothesSlots clothesItems in travellerClothes)
        {
            if (clothesItems != null)
                clothes.InstantiatePieceOfClothing(clothesItems.model, travelerSpawner.newTraveler);
        }
    }

    private void GiveOneIdenticalPieceOfClothing()
    {
        travellerClothes = new ClothesSlots[Enum.GetValues(typeof(BodySlot)).Length];
        BodySlot? selectedBodySlot;
        ClothesSlots? selectedPieceOfClothing = null;

        while (selectedPieceOfClothing == null)
        { 
            int randomIndex = Random.Range(0, identifyingThief.thiefClothes.Length);
            selectedPieceOfClothing = identifyingThief.thiefClothes[randomIndex];

        }
        
        selectedBodySlot = selectedPieceOfClothing.slot;
        travellerClothes[(int)selectedBodySlot] = selectedPieceOfClothing;

        foreach (BodySlot bodySlot in Enum.GetValues(typeof(BodySlot)))
        {
            ClothesSlots newPieceOfClothing = null;
            if (bodySlot != selectedBodySlot)
            {
                if (bodySlot == BodySlot.Head)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomHeadItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[0]);
                }
                else if (bodySlot == BodySlot.Torso)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomTorsoItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[1]);
                }
                else if (bodySlot == BodySlot.Legs)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomLegsItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[2]);
                }
                else if (bodySlot == BodySlot.Accessories)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomAccessoriesItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[3]);
                }
                
                if (newPieceOfClothing != null)
                    travellerClothes[(int)bodySlot] = newPieceOfClothing;
            }
        }
        
        foreach (ClothesSlots clothesItems in travellerClothes)
        {
            if (clothesItems != null)
                clothes.InstantiatePieceOfClothing(clothesItems.model, travelerSpawner.newTraveler);
        }
    }

    private void GiveTwoIdenticalClothes()
    {
        travellerClothes = new ClothesSlots[Enum.GetValues(typeof(BodySlot)).Length];
        ClothesSlots selectedPieceOfClothing = null;
        
        List<ClothesSlots> selectedPieces = new List<ClothesSlots>();
        List<BodySlot> selectedBodySlots = new List<BodySlot>();

        while (selectedPieces.Count < 2)
        {
            while (selectedPieceOfClothing == null)
            {
                int randomIndex = Random.Range(0, identifyingThief.thiefClothes.Length);
                selectedPieceOfClothing = identifyingThief.thiefClothes[randomIndex];
            }
            if (!selectedPieces.Contains(selectedPieceOfClothing))
                selectedPieces.Add(selectedPieceOfClothing);
            selectedPieceOfClothing = null;
        }

        foreach (ClothesSlots selectedPiece in selectedPieces)
        {
            travellerClothes[(int)selectedPiece.slot] = selectedPiece;
            selectedBodySlots.Add(selectedPiece.slot);
        }
        
        foreach (BodySlot bodySlot in Enum.GetValues(typeof(BodySlot)))
        {
            ClothesSlots newPieceOfClothing = null;
            if (!selectedBodySlots.Contains(bodySlot))
            {
                if (bodySlot == BodySlot.Head)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomHeadItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[0]);
                }
                else if (bodySlot == BodySlot.Torso)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomTorsoItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[1]);
                }
                else if (bodySlot == BodySlot.Legs)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomLegsItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[2]);
                }
                else if (bodySlot == BodySlot.Accessories)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomAccessoriesItem();}
                    while (newPieceOfClothing == identifyingThief.thiefClothes[3]);
                }
                
                if (newPieceOfClothing != null)
                    travellerClothes[(int)bodySlot] = newPieceOfClothing;
            }
        }
        
        foreach (ClothesSlots clothesItems in travellerClothes)
        {
            if (clothesItems != null)
                clothes.InstantiatePieceOfClothing(clothesItems.model, travelerSpawner.newTraveler);
        }
    }

    public void SpawnClothes(int i)
    {
        if (i <= travelerSpawner.travelerCount * 0.3)
            GivingNonEqualClothes();
        else if (i > travelerSpawner.travelerCount * 0.3 && i <= travelerSpawner.travelerCount * 0.6)
            GiveOneIdenticalPieceOfClothing();
        else
            GiveTwoIdenticalClothes();
    }
}
