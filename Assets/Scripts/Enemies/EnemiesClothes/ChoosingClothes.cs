using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingClothes : MonoBehaviour
{
    [field: Header("Clothes")]
    [SerializeField] private ClothesSlots[] clothesSlots;
    public ClothesSlots headPiece {get; set;}
    public ClothesSlots torsoPiece {get; set;}
    public ClothesSlots legsPiece {get; set;}
    public ClothesSlots accessoriesPiece {get; set;}
    
    public void HeadSlot()
    {
        List<ClothesSlots> headItems = new List<ClothesSlots>();
        foreach (ClothesSlots pieces in clothesSlots)
        {
            if (pieces.slot == BodySlot.Head)
                headItems.Add(pieces);
        }

        if (headItems.Count > 0  && Random.value > 0.3f)
        {
            int headRandomIndex = Random.Range(0, headItems.Count);
            headPiece = headItems[headRandomIndex];
        }
    }

    public void TorsoSlot()
    {
        List<ClothesSlots> torsoItems = new List<ClothesSlots>();
        foreach (ClothesSlots pieces in clothesSlots)
        {
            if (pieces.slot == BodySlot.Torso)
                torsoItems.Add(pieces);
        }

        if (torsoItems.Count > 0)
        {
            int torsoRandomIndex = Random.Range(0, torsoItems.Count);
            torsoPiece = torsoItems[torsoRandomIndex];
        }
    }

    public void LegsSlot()
    {
        List<ClothesSlots> legItems = new List<ClothesSlots>();
        foreach (ClothesSlots pieces in clothesSlots)
        {
            if(pieces.slot == BodySlot.Legs)
                legItems.Add(pieces);
        }

        if (legItems.Count > 0)
        {
            int legRandomIndex = Random.Range(0, legItems.Count);
            legsPiece = legItems[legRandomIndex];
        }
    }

    public void AccessoriesSlot()
    {
        List<ClothesSlots> accessoriesItems = new List<ClothesSlots>();
        foreach (ClothesSlots pieces in clothesSlots)
        {
            if (pieces.slot == BodySlot.Accessories)
                accessoriesItems.Add(pieces);
        }

        if (accessoriesItems.Count > 0  && Random.value > 0.3f)
        {
            int firstAccessoryRandomIndex = Random.Range(0, accessoriesItems.Count);
            accessoriesPiece = accessoriesItems[firstAccessoryRandomIndex];
            //GameObject itemOfClothing = Instantiate(accessoriesPiece.model);

            if (accessoriesPiece.name == "trolley" && Random.value > 0.5f) //has a 50% change of getting the backpack
            {
                ClothesSlots backpack = accessoriesItems.Find(x => x.name == "backpack");
                if (backpack != null)
                {
                    GameObject backpackItem = Instantiate(backpack.model);
                }
            }
            else if (accessoriesPiece.name == "backpack" && Random.value > 0.5f) //has a 50% change of getting the trolley
            {
                ClothesSlots trolley = accessoriesItems.Find(x => x.name == "trolley");
                if (trolley != null)
                {
                    GameObject trolleyItem = Instantiate(trolley.model);
                }
            }
        }
    }
}
