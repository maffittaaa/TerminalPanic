using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirportMode
{
    Normal,
    Panic
}

public class TravelerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject travelerPrefab;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private Vector3 spawnSize = new Vector3(10f, 0f, 10f);

    [SerializeField] private int travelerCount = 10;

    [SerializeField] private List<GameObject> currentTravelers = new List<GameObject>();
    [SerializeField] private List<TravelerAI> travelerAIs = new List<TravelerAI>();
    [SerializeField] public AirportMode currentMode { get; private set; }

    void Start()
    {
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
            SpawnTraveler();
        }
    }

    void SpawnTraveler()
    {
        if (travelerPrefab == null) return;

        Vector3 randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

        Vector3 randomPosition = new Vector3(
            Random.Range(randomSpawnPoint.x - spawnSize.x / 2, randomSpawnPoint.x + spawnSize.x / 2),
            randomSpawnPoint.y,
            Random.Range(randomSpawnPoint.z - spawnSize.z / 2, randomSpawnPoint.z + spawnSize.z / 2)
        );

        GameObject newTraveler = Instantiate(travelerPrefab, randomPosition, Quaternion.identity);
        currentTravelers.Add(newTraveler);

        TravelerAI travelerAI = newTraveler.GetComponent<TravelerAI>();

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
        Debug.Log("Airport mode set to: " + currentMode);

        // inform all travelers
        TravelerSpawner spawner = FindObjectOfType<TravelerSpawner>();
        if (spawner != null)
        {
            foreach (TravelerAI traveler in spawner.travelerAIs)
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

}
