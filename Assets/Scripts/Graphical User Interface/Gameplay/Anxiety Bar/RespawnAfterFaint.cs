using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterFaint : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private GettingOutOfSafeSpace gettingOutOfSafeSpace;

    private void Start()
    {
        if (!anxietyBar.isCoroutineRunning && gettingOutOfSafeSpace.Collided(true))
        {
            anxietyBar.StartCoroutine(anxietyBar.AnxietyLevelsUp());
            anxietyBar.isCoroutineRunning = true;
        }
    }

    private void Update()
    {
        if (anxietyBar.currentAnxiety == anxietyBar.maxAnxiety && anxietyBar.isCoroutineRunning) //better way please...
        {
            anxietyBar.isCoroutineRunning = false;
            anxietyBar.StopCoroutine(anxietyBar.AnxietyLevelsUp());
            Debug.Log("Coroutine: " + anxietyBar.isCoroutineRunning);
            Debug.Log("Collided: " + gettingOutOfSafeSpace.Collided(true));
            this.transform.position = new Vector3(27.53f,3.79f,-301.08f);
            anxietyBar.currentAnxiety = 0f;
        }
    }
}
