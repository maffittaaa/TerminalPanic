using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Flickering[] lights;

    void Start()
    {
        lights = FindObjectsByType<Flickering>(FindObjectsSortMode.None);
    }

    public void TurnOnAndOffManager()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].TurnOnAndOff();
        }
    }
}
