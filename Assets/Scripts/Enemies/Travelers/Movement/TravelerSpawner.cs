using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum IAirportMode
{
    Normal,
    Panic
}

public class TravelerSpawner : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject travelerPrefab;
    [SerializeField] private GameObject travellersHolder;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private Vector3 spawnSize = new Vector3(10f, 0f, 10f);
    public EnemiesClothes enemiesClothes;
    public int travelerCount = 10;

    [SerializeField] private List<GameObject> currentTravelers = new List<GameObject>();
    [SerializeField] private List<TravelerAI> travelerAIs = new List<TravelerAI>();
    [SerializeField] public IAirportMode currentMode { get; private set; }

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
        newTraveler.tag = "Enemy";

        TravelerAI travelerAI = newTraveler.GetComponent<TravelerAI>();
        //enemiesClothes.clothesAndColors = newTraveler.GetComponent<ClothesAndColors>();
        //SpawningClothes(i);
        //visualClothes.travelerSpawner = this;

        TravelerType randomType = (TravelerType)Random.Range(0, Enum.GetValues(typeof(TravelerType)).Length);

        travelerAI.type = randomType;

        if (travelerAI != null)
        {
            travelerAIs.Add(travelerAI); 

            if (currentMode == IAirportMode.Normal)
            {
                if (Random.value > 0.5f)
                    travelerAI.SetState(TravelerState.Wandering);
                else
                    travelerAI.SetState(TravelerState.Waiting);
            }
            else if (currentMode == IAirportMode.Panic)
                travelerAI.SetState(TravelerState.Waiting);
        }
    }

    public void SwitchToPanicMode()
    {
        SetAirportMode(IAirportMode.Panic);
    }

    public void SwitchToNormalMode()
    {
        SetAirportMode(IAirportMode.Normal);
    }

    public void SetAirportMode(IAirportMode mode)
    {
        currentMode = mode;
        //Debug.Log("Airport mode set to: " + currentMode);

        // inform all travelers
        if (travelerAIs != null)
        {
            foreach (TravelerAI traveler in travelerAIs)
            {
                if (traveler)
                    traveler.OnAirportModeChanged(mode);
            }
        }
    }

    // private void SpawningClothes(int i)
    // {
    //     if (i <= travelerCount * 0.2)
    //         enemiesClothes.GivingClothesToEnemies();
    //     else if (i > travelerCount * 0.2 && i <= travelerCount * 0.6)
    //         enemiesClothes.MatchingWithOneCloth();
    //     else
    //         enemiesClothes.MatchingWithTwoClothes();
    // }
}
