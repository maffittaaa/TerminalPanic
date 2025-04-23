using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelerSpawner : MonoBehaviour
{
    public GameObject travelerPrefab;

    public Vector3 spawnCenter = Vector3.zero;
    public Vector3 spawnSize = new Vector3(10f, 0f, 10f); 

    public float spawnRate = 5f;
    public int maxTravelers = 10;

    private float spawnTimer;
    private List<GameObject> currentTravelers = new List<GameObject>();

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate && currentTravelers.Count < maxTravelers)
        {
            SpawnTraveler();
            spawnTimer = 0f;
        }

    }

    void SpawnTraveler()
    {
        if (travelerPrefab == null) return;

        Vector3 randomPosition = new Vector3(
            Random.Range(spawnCenter.x - spawnSize.x / 2, spawnCenter.x + spawnSize.x / 2),
            spawnCenter.y, 
            Random.Range(spawnCenter.z - spawnSize.z / 2, spawnCenter.z + spawnSize.z / 2)
        );

        GameObject newTraveler = Instantiate(travelerPrefab, randomPosition, Quaternion.identity);
        currentTravelers.Add(newTraveler);
    }
}

