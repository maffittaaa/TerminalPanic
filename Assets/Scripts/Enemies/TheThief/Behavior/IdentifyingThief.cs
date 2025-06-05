using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdentifyingThief : MonoBehaviour
{
    [field: Header("Thief")]
    [SerializeField] public GameObject thief;
    [SerializeField] private ThiefClothes thiefClothes;
    private Vector3 thiefPosition;
    [SerializeField] private float distanceToEnemy;
    
    [field: Header("Clues")]
    [SerializeField] private ClueText clue;
    public ChoosingClothes hintsForThief;
    public int peopleNeededToNextClue;
    private int peeopleCountToNextClue = 0;
    private Dictionary<GameObject, GameObject> peopleSeen = new Dictionary<GameObject, GameObject>();
    [SerializeField] private EnemiesClothes enemiesClothes;
    [SerializeField] private GameObject travelerPrefab;

    [SerializeField] private TravelerSpawner travelerSpawner;
    
    [field: Header("Scripts")]
    [SerializeField] private Camera playerCamera;
    
    private void Awake()
    {
        thiefPosition = new Vector3(Random.Range(0, -109f), 19.10258f, Random.Range(-3f, 69f));
        thief = Instantiate(thief, thiefPosition, Quaternion.identity);
        
        hintsForThief = FindObjectOfType<ChoosingClothes>();
    }

    public bool CluesMatchThiefsClothes(ClothesSlots[] thiefClothes)
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
    
    public void AreTheseClothesEqualToTheThiefsClothes(TravelerAI travelerAI)
    {
        enemiesClothes.travelerClothes = travelerAI.GetComponentInChildren<EnemyClothes>().travelerClothes;
        thiefClothes = (ThiefClothes)FindObjectOfType(typeof(ThiefClothes));
        bool matchingClothes = false;

        for (int i = 0; i < enemiesClothes.travelerClothes.Length; i++)
        {
            if (enemiesClothes.travelerClothes[i] == clue.clues[clue.numberOfClues] && peeopleCountToNextClue < peopleNeededToNextClue)
            {
                matchingClothes = true;
                peeopleCountToNextClue++;
            }
        }
        
        if (!matchingClothes)
        {
            int randomNum = Random.Range(0, transform.childCount);

            Vector3 randomSpawnPoint = travelerSpawner.spawnPoints[randomNum].transform.position;
                
            Vector3 randomPosition = new Vector3(
                Random.Range(randomSpawnPoint.x - travelerSpawner.spawnSize.x / 2, randomSpawnPoint.x + travelerSpawner.spawnSize.x / 2),
                randomSpawnPoint.y,
                Random.Range(randomSpawnPoint.z - travelerSpawner.spawnSize.z / 2, randomSpawnPoint.z + travelerSpawner.spawnSize.z / 2)
            );
                
            GameObject traveler = Instantiate(travelerPrefab, randomPosition , Quaternion.identity, travelerSpawner.travellersHolder.transform);
            traveler.GetComponentInChildren<EnemyClothes>().travelerClothes = enemiesClothes.travelerClothes;
        }
    }
}
