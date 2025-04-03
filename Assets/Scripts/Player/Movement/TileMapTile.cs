using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

public class TileMapTile : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    public GameObject currentTile { get; private set; }
    private GameObject tileBefore;

    public UnityEvent tileChanged;

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0,-1,0), out hit, 5, mask))
        {
            currentTile = hit.collider.gameObject;

            if (currentTile != tileBefore)
            {
                tileBefore = currentTile;
                tileChanged.Invoke();
            }
        }
    }
}
