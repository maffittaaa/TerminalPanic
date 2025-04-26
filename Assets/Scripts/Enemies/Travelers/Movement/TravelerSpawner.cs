using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AirportMode
{
    Normal,
    Panic
}

public class TravelerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject travelerPrefab;
    [SerializeField] private GameObject travellersHolder;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private Vector3 spawnSize = new Vector3(10f, 0f, 10f);
    private EnemiesClothes enemiesClothes;
    [SerializeField] private int travelerCount = 10;

    [SerializeField] private List<GameObject> currentTravelers = new List<GameObject>();
    [SerializeField] private List<TravelerAI> travelerAIs = new List<TravelerAI>();
    [SerializeField] public AirportMode currentMode { get; private set; }

    void Start()
    {
        enemiesClothes = GetComponent<EnemiesClothes>();
        spawnPoints = GetAllChilds();
        SpawnAllTravelers();
    }


    private GameObject[] GetAllChilds()
    {
        GameObject[] childs = new GameObject[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i).gameObject;
        }
        return childs;
    }

    void SpawnAllTravelers()
    {
        for (int i = 0; i < travelerCount; i++)
        {
            SpawnTraveler(i);
        }
    }

    void SpawnTraveler(int i)
    {
        if (travelerPrefab == null) return;

        int randomNum = Random.Range(0, transform.childCount);

        Vector3 randomSpawnPoint = spawnPoints[randomNum].transform.position;

        Vector3 randomPosition = new Vector3(
            Random.Range(randomSpawnPoint.x - spawnSize.x / 2, randomSpawnPoint.x + spawnSize.x / 2),
            randomSpawnPoint.y,
            Random.Range(randomSpawnPoint.z - spawnSize.z / 2, randomSpawnPoint.z + spawnSize.z / 2)
        );

        GameObject newTraveler = Instantiate(travelerPrefab, randomPosition, Quaternion.identity, travellersHolder.transform);
        currentTravelers.Add(newTraveler);
        TravelerAI travelerAI = newTraveler.GetComponent<TravelerAI>();
        enemiesClothes.clothesAndColors = newTraveler.GetComponent<ClothesAndColors>();
        SpawningClothes(i);

        TravelerType randomType = (TravelerType)Random.Range(0, Enum.GetValues(typeof(TravelerType)).Length);

        travelerAI.type = randomType;

        travelerAI.InitWithSpawner(this);

        if (travelerAI != null)
        {
            travelerAIs.Add(travelerAI); 

            if (currentMode == AirportMode.Normal)
            {
                if (Random.value > 0.5f)
                    travelerAI.SetState(TravelerState.Wandering);
                else
                    travelerAI.SetState(TravelerState.Waiting);
            }
            else if (currentMode == AirportMode.Panic)
            {
                travelerAI.SetState(TravelerState.Waiting);
            }
        }
    }


    public void SetAirportMode(AirportMode mode)
    {
        currentMode = mode;
        //Debug.Log("Airport mode set to: " + currentMode);

        // inform all travelers
        if (travelerAIs != null)
        {
            foreach (TravelerAI traveler in travelerAIs)
            {
                traveler.OnAirportModeChanged(mode);
            }
        }
    }

    void SwitchToPanicMode()
    {
        SetAirportMode(AirportMode.Panic);
    }

    void SwitchToNormalMode()
    {
        SetAirportMode(AirportMode.Normal);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchToPanicMode();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SwitchToNormalMode();
        }
    }

    private void SpawningClothes(int i)
    {
        if (i <= 2)
            enemiesClothes.GivingClothesTo2Enemies();
        else if (i > 2 && i <= 4)
            enemiesClothes.MatchingWithOneCloth();
        else
            enemiesClothes.MatchingWithTwoClothes();
    }
}
