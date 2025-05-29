using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdentifyingThief : MonoBehaviour
{
    [field: Header("Thief")]
    [SerializeField] public GameObject thief;
    private Vector3 thiefPosition;
    [SerializeField] private float distanceToEnemy;
    public List<ClothesSlots> thiefClothes = new List<ClothesSlots>();
    
    [field: Header("Clues")]
    [SerializeField] private ClueText clue;
    public ClothesSlots hintsForThief;
    [SerializeField] private int peopleNeededToNextClue;
    private int currentPeopleToNextClue = 0;
    private Dictionary<GameObject, GameObject> peopleSeen = new Dictionary<GameObject, GameObject>();

    [field: Header("Clothes")]
    [SerializeField] private ChoosingClothes clothes;
    
    [field: Header("Scripts")]
    [SerializeField] private Camera playerCamera;
    
    private void Awake()
    {
        thiefPosition = new Vector3(Random.Range(0, -109f), 19.10258f, Random.Range(-3f, 69f));
        thief = Instantiate(thief, thiefPosition, Quaternion.identity);
        hintsForThief = thief.GetComponent<ClothesSlots>();
        
        clothes.HeadSlot();
        clothes.TorsoSlot();
        clothes.LegsSlot();
        clothes.AccessoriesSlot();
        
        thiefClothes.Add(clothes.headPiece);
        thiefClothes.Add(clothes.torsoPiece);
        thiefClothes.Add(clothes.legsPiece);
        thiefClothes.Add(clothes.accessoriesPiece);
        Debug.Log("head: " + thiefClothes[0]);
        Debug.Log("head: " + thiefClothes[1]);
        Debug.Log("head: " + thiefClothes[2]);
        Debug.Log("head: " + thiefClothes[3]);
        
        foreach (ClothesSlots item in thiefClothes)
            Instantiate(item.model, thiefPosition, Quaternion.identity);
    }
    
    public bool DoesTheThiefHasThis(List<ClothesSlots> clothesSlots)
    {
        int matchingOutfits = 0;
        
        for (int i = 0; i < clothesSlots.Count; i++)
        {
            for (int j = 0; j < clue.numberOfClues + 1; j++)
            {
                if (clothesSlots[i] == clue.clues[j])
                    matchingOutfits++;
            }
        }

        if (matchingOutfits >= clue.numberOfClues)
            return true;
        return false;
    }
    
    public void SeeingThief()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, distanceToEnemy))
        {
            if (hit.collider.gameObject.CompareTag("Enemy") && !peopleSeen.ContainsKey(hit.collider.gameObject))
            {
                List<ClothesSlots> tempListClothes = hit.collider.gameObject.GetComponent<List<ClothesSlots>>();

                if (DoesTheThiefHasThis(tempListClothes))
                {
                    currentPeopleToNextClue++;
                    peopleSeen.Add(hit.collider.gameObject, hit.collider.gameObject);
                    if (currentPeopleToNextClue >= peopleNeededToNextClue)
                    {
                        clue.TextForClue();
                        currentPeopleToNextClue = 0;
                        peopleSeen.Clear();
                    }
                }
            }
        }
    }
    
    private void FixedUpdate()
    {
        SeeingThief();
    }
}
