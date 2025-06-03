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
    
    public ClothesSlots ChooseRandomHeadItem()
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
        else if (headPiece == null)
            Debug.Log("No head item");
        return headPiece;
    }

    public ClothesSlots ChooseRandomTorsoItem()
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
        return torsoPiece;
    }

    public ClothesSlots ChooseRandomLegsItem()
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
        return legsPiece;
    }

    public ClothesSlots ChooseRandomAccessoriesItem()
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
        else if (accessoriesPiece == null)
            Debug.Log("No accessories");

        return accessoriesPiece;
    }

    public void InstantiatePieceOfClothing(GameObject item, GameObject prefab)
    {
        GameObject pieceOfClothing = Instantiate(item);
        pieceOfClothing.transform.SetParent(prefab.transform);
        pieceOfClothing.transform.localPosition = Vector3.zero;
        pieceOfClothing.transform.localRotation = Quaternion.Euler(new Vector3(270, 0, 270));
        if (pieceOfClothing.gameObject.name.Contains("backpack"))
        {
            pieceOfClothing.transform.localPosition = new Vector3(0, 0, -0.117f);
            pieceOfClothing.transform.localRotation = Quaternion.Euler(-90, 0, 90);
        }
        else if (pieceOfClothing.gameObject.name.Contains("headphones"))
            pieceOfClothing.transform.localPosition = new Vector3(0, 0, -0.096f);
    }
}
