using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesClothes : MonoBehaviour
{
    [SerializeField] private ThiefClothes thief;
    [SerializeField] private ChoosingClothes clothes;
    public ClothesSlots[] travelerClothes;
    [SerializeField] private TravelerSpawner travelerSpawner;
    public GameObject[] clothesModels;

    private void Start()
    {
        thief = (ThiefClothes)FindObjectOfType(typeof(ThiefClothes));
    }

    private void Update()
    {
        if (thief == null)
            thief = (ThiefClothes)FindObjectOfType(typeof(ThiefClothes));
    }

    private void GivingNonEqualClothes(int k)
    {
        if (thief == null)
            thief = (ThiefClothes)FindObjectOfType(typeof(ThiefClothes));
        
        bool validClothes = false;
        travelerClothes = new ClothesSlots[Enum.GetValues(typeof(BodySlot)).Length];
        
        while (!validClothes)
        {
            clothes.ChooseRandomHeadItem();
            clothes.ChooseRandomTorsoItem();
            clothes.ChooseRandomLegsItem();
            clothes.ChooseRandomAccessoriesItem();
            
            validClothes = clothes.headPiece != thief.thiefClothes[0] &&
                           clothes.torsoPiece != thief.thiefClothes[1] &&
                           clothes.legsPiece != thief.thiefClothes[2] &&
                           clothes.accessoriesPiece != thief.thiefClothes[3];
        }

        if (validClothes)
        {
            travelerClothes[0] = clothes.headPiece;
            travelerClothes[1] = clothes.torsoPiece;
            travelerClothes[2] = clothes.legsPiece;
            travelerClothes[3] = clothes.accessoriesPiece;
        }

        for (int i = 0; i < travelerClothes.Length; i++)
        {
            for (int j = 0; j < clothesModels.Length; j++)
            {
                if (travelerClothes[i] != null && clothesModels[j] != null && travelerClothes[i].id == j)
                {
                    clothesModels[j].SetActive(true);
                    travelerClothes[i].model = clothesModels[j];
                }
            }
        }
        
        travelerSpawner.currentTravelers[k].GetComponentInChildren<EnemyClothes>().travelerClothes = travelerClothes;
    }

    private void GiveOneIdenticalPieceOfClothing(int v)
    {
        travelerClothes = new ClothesSlots[Enum.GetValues(typeof(BodySlot)).Length];
        BodySlot? selectedBodySlot;
        ClothesSlots selectedPieceOfClothing = null;

        while (selectedPieceOfClothing == null)
        { 
            int randomIndex = Random.Range(0, thief.thiefClothes.Length);
            selectedPieceOfClothing = thief.thiefClothes[randomIndex];

        }
        
        selectedBodySlot = selectedPieceOfClothing.slot;
        travelerClothes[(int)selectedBodySlot] = selectedPieceOfClothing;

        foreach (BodySlot bodySlot in Enum.GetValues(typeof(BodySlot)))
        {
            ClothesSlots newPieceOfClothing = null;
            if (bodySlot != selectedBodySlot)
            {
                if (bodySlot == BodySlot.Head)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomHeadItem();}
                    while (newPieceOfClothing == thief.thiefClothes[0]);
                }
                else if (bodySlot == BodySlot.Torso)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomTorsoItem();}
                    while (newPieceOfClothing == thief.thiefClothes[1]);
                }
                else if (bodySlot == BodySlot.Legs)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomLegsItem();}
                    while (newPieceOfClothing == thief.thiefClothes[2]);
                }
                else if (bodySlot == BodySlot.Accessories)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomAccessoriesItem();}
                    while (newPieceOfClothing == thief.thiefClothes[3]);
                }
                
                if (newPieceOfClothing != null)
                    travelerClothes[(int)bodySlot] = newPieceOfClothing;
            }
        }
        
        for (int i = 0; i < travelerClothes.Length; i++)
        {
            for (int j = 0; j < clothesModels.Length; j++)
            {
                if (travelerClothes[i] != null && clothesModels[j] != null && travelerClothes[i].id == j)
                {
                    clothesModels[j].SetActive(true);
                    travelerClothes[i].model = clothesModels[j];
                }
            }
        }
        
        travelerSpawner.currentTravelers[v].GetComponentInChildren<EnemyClothes>().travelerClothes = travelerClothes;
    }

    private void GiveTwoIdenticalClothes(int v)
    {
        travelerClothes = new ClothesSlots[Enum.GetValues(typeof(BodySlot)).Length];
        ClothesSlots selectedPieceOfClothing = null;
        
        List<ClothesSlots> selectedPieces = new List<ClothesSlots>();
        List<BodySlot> selectedBodySlots = new List<BodySlot>();

        while (selectedPieces.Count < 2)
        {
            while (selectedPieceOfClothing == null)
            {
                int randomIndex = Random.Range(0, thief.thiefClothes.Length);
                selectedPieceOfClothing = thief.thiefClothes[randomIndex];
            }
            if (!selectedPieces.Contains(selectedPieceOfClothing))
                selectedPieces.Add(selectedPieceOfClothing);
            selectedPieceOfClothing = null;
        }

        foreach (ClothesSlots selectedPiece in selectedPieces)
        {
            travelerClothes[(int)selectedPiece.slot] = selectedPiece;
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
                    while (newPieceOfClothing == thief.thiefClothes[0]);
                }
                else if (bodySlot == BodySlot.Torso)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomTorsoItem();}
                    while (newPieceOfClothing == thief.thiefClothes[1]);
                }
                else if (bodySlot == BodySlot.Legs)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomLegsItem();}
                    while (newPieceOfClothing == thief.thiefClothes[2]);
                }
                else if (bodySlot == BodySlot.Accessories)
                {
                    do {newPieceOfClothing = clothes.ChooseRandomAccessoriesItem();}
                    while (newPieceOfClothing == thief.thiefClothes[3]);
                }
                
                if (newPieceOfClothing != null)
                    travelerClothes[(int)bodySlot] = newPieceOfClothing;
            }
        }
        
        for (int i = 0; i < travelerClothes.Length; i++)
        {
            for (int j = 0; j < clothesModels.Length; j++)
            {
                if (travelerClothes[i] != null && clothesModels[j] != null && travelerClothes[i].id == j)
                {
                    clothesModels[j].SetActive(true);
                    travelerClothes[i].model = clothesModels[j];
                }
            }
        }
        
        travelerSpawner.currentTravelers[v].GetComponentInChildren<EnemyClothes>().travelerClothes = travelerClothes;
    }

    public void SpawnClothes(int i)
    {
        clothesModels = travelerSpawner.currentTravelers[i].GetComponentInChildren<EnemyClothes>().travelerPossibleClothes;
        if (i <= travelerSpawner.travelerCount * 0.3)
            GivingNonEqualClothes(i);
        else if (i > travelerSpawner.travelerCount * 0.3 && i <= travelerSpawner.travelerCount * 0.6)
            GiveOneIdenticalPieceOfClothing(i);
        else
            GiveTwoIdenticalClothes(i);
    }
    
}
