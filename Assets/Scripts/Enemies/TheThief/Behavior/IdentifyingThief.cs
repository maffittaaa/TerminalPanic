using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdentifyingThief : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject thief;
    [SerializeField] private ClueText clue;
    public List<EClothesAndAccessoriesTypes> thiefClothes = new List<EClothesAndAccessoriesTypes>();
    public List<EColorTypes> thiefClothesColor = new List<EColorTypes>();
    private Dictionary<GameObject, GameObject> peopleSeen = new Dictionary<GameObject, GameObject>();
    public HintFinder hintsForThief;
    [SerializeField] private int peopleNeededToNextClue;
    private int currentPeopleToNextClue = 0;
    private int clothesAndAccessoriesNumber;
    [SerializeField] private float distanceToEnemy;
    
    private void Awake()
    {
        thief = Instantiate(thief, new Vector3(0, 0, 0), Quaternion.identity); //instantiate the thief into the world
        hintsForThief = thief.GetComponent<HintFinder>(); //add the hint finder component
        
        clothesAndAccessoriesNumber = Random.Range(3, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
        int i = 0;
        int k = 0;
        
        while (i < clothesAndAccessoriesNumber)
        {
            bool equalType = false;
            if (k >= 1000)
            {
                Debug.Log("something went wrong");
                break;
            }
            
            EClothesAndAccessoriesTypes clothesType = (EClothesAndAccessoriesTypes)Random.Range(1, Enum.GetValues(typeof(EClothesAndAccessoriesTypes)).Length - 1);
            for (int j = 0; j < hintsForThief.clothesAndAccessoriesTypes.Count; j++)
            {
                if (clothesType == hintsForThief.clothesAndAccessoriesTypes[j])
                {
                    equalType = true;
                    break;
                }
            }

            if (!equalType)
            {
                EColorTypes colorsType = (EColorTypes)Random.Range(1, Enum.GetValues(typeof(EColorTypes)).Length - 1);
                hintsForThief.clothesAndAccessoriesTypes.Add(clothesType);
                hintsForThief.colorTypes.Add(colorsType);
                i++;
            }
            k++;
        }

        for (i = 0; i < clothesAndAccessoriesNumber; i++)
        {
            thiefClothes.Add(hintsForThief.clothesAndAccessoriesTypes[i]);
            thiefClothesColor.Add(hintsForThief.colorTypes[i]);
        }
    }
    
    public bool DoesTheThiefHasThis(List<EClothesAndAccessoriesTypes> clothesAndAccessoriesType, List<EColorTypes> colorType)
    {
        int matchingOutfits = 0;
        
        for (int i = 0; i < clothesAndAccessoriesType.Count; i++)
        {
            for (int j = 0; j < clue.numberOfClues + 1; j++)
            {
                if (clothesAndAccessoriesType[i] == clue.clothesClues[j] && colorType[i] == clue.colorClues[j])
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
            //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            if (hit.collider.gameObject.CompareTag("Enemy") && !peopleSeen.ContainsKey(hit.collider.gameObject))
            {
                List<EClothesAndAccessoriesTypes> tempListClothes = hit.collider.gameObject.GetComponent<HintFinder>().clothesAndAccessoriesTypes;
                List<EColorTypes> tempListColor = hit.collider.gameObject.GetComponent<HintFinder>().colorTypes;

                if (DoesTheThiefHasThis(tempListClothes, tempListColor))
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
