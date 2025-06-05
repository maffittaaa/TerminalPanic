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
    
    [field: Header("Scripts")]
    [SerializeField] private Camera playerCamera;
    
    private void Awake()
    {
        thiefPosition = new Vector3(Random.Range(0, -109f), 19.10258f, Random.Range(-3f, 69f));
        thief = Instantiate(thief, thiefPosition, Quaternion.identity);
        
        hintsForThief = FindObjectOfType<ChoosingClothes>();
    }
    
    public bool DoesTheThiefHasThis(ClothesSlots[] thiefClothes)
    {
        int matchingOutfits = 0;
        
        for (int i = 0; i < thiefClothes.Length; i++)
        {
            for (int j = 0; j < clue.numberOfClues + 1; j++)
            {
                if (thiefClothes[i] == clue.clues[j])
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
                ClothesSlots[] tempListClothes = hit.collider.gameObject.GetComponentInChildren<EnemyClothes>().travelerClothes;

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
