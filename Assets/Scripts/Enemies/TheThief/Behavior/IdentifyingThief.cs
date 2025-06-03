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
    
    [field: Header("Clues")]
    [SerializeField] private ClueText clue;
    public ChoosingClothes hintsForThief;
    [SerializeField] private int peopleNeededToNextClue;
    private int currentPeopleToNextClue = 0;
    private Dictionary<GameObject, GameObject> peopleSeen = new Dictionary<GameObject, GameObject>();

    [field: Header("Clothes")]
    [SerializeField] private ChoosingClothes clothes;
    public ClothesSlots[] thiefClothes;
    
    [field: Header("Scripts")]
    [SerializeField] private Camera playerCamera;
    
    private void Awake()
    {
        thiefPosition = new Vector3(Random.Range(0, -109f), 19.10258f, Random.Range(-3f, 69f));
        thief = Instantiate(thief, thiefPosition, Quaternion.identity);
        hintsForThief = FindObjectOfType<ChoosingClothes>();
        
        clothes.ChooseRandomHeadItem();
        thiefClothes[0] = clothes.headPiece;
        if (clothes.headPiece == null)
            clothes.ChooseRandomHeadItem();
            
        Debug.Log("0: " + thiefClothes[0]);
        
        clothes.ChooseRandomTorsoItem();
        thiefClothes[1] = clothes.torsoPiece;
        Debug.Log("1: " + thiefClothes[1]);
        
        clothes.ChooseRandomLegsItem();
        thiefClothes[2] = clothes.legsPiece;
        Debug.Log("2: " + thiefClothes[2]);
        
        clothes.ChooseRandomAccessoriesItem();
        thiefClothes[3] = clothes.accessoriesPiece;
        Debug.Log("3: " + thiefClothes[3]);
        
        foreach (ClothesSlots item in thiefClothes)
        {
            if (item != null)
                clothes.InstantiatePieceOfClothing(item.model, thief);
        }
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
