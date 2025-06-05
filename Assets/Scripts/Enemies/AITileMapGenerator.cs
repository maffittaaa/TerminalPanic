using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITileMapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] floor;
    [SerializeField] private GameObject tileParent;
    [SerializeField] private LayerMask obstaclesMask;
    [SerializeField] private GameObject prefab;
    [field: SerializeField] public float size { get; private set; }
    [SerializeField] private float tilesX;
    [SerializeField] private float tilesZ;
    [field: SerializeField] public List<GameObject> tiles { get; private set; }

    void Awake()
    {
        GenerateTileMap();
    }

    private void GenerateTileMap()
    {
        for(int i = 0; i < floor.Length; i++)
        {
            tilesX = floor[i].transform.localScale.x / size;
            tilesZ = floor[i].transform.localScale.z / size;
            prefab.transform.localScale = new Vector3(size, 0.5f, size);
            Vector3 startingPoint = new Vector3(floor[i].transform.position.x + (floor[i].transform.localScale.x/2 + size/2), floor[i].transform.position.y, floor[i].transform.position.z + (floor[i].transform.localScale.z / 2 + size/2));

            for (int x = 1; x <= tilesX; x++)
            {
                for (int z = 1; z <= tilesZ; z++)
                {
                    Vector3 newPos = new Vector3(startingPoint.x - (x * size), startingPoint.y, startingPoint.z - (z * size));
                    GameObject newTile = Instantiate(prefab, newPos, transform.rotation, tileParent.transform);
                    tiles.Add(newTile);

                    RaycastHit hit;
                    if (Physics.Raycast(newTile.transform.position, new Vector3(0, 1, 0), out hit, 10, obstaclesMask))
                    {
                        newTile.tag = "Obstacle";
                        Destroy(newTile);
                    }
                }
            }
        }
    }
}
