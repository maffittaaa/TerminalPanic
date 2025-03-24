using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterFaint : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private GettingOutOfSafeSpace gettingOutOfSafeSpace;
    
    private void Update()
    {
        if (anxietyBar.currentAnxiety == anxietyBar.maxAnxiety && anxietyBar.CouroutineIsRunning(true)) //better way please...
        {
            anxietyBar.StopCoroutine(anxietyBar.AnxietyLevelsUp());
            anxietyBar.CouroutineIsRunning(false);
            Debug.Log(anxietyBar.CouroutineIsRunning(false));
            this.transform.position = new Vector3(27.53f,3.79f,-301.08f);
            anxietyBar.currentAnxiety = 0f;
        }
    }
}
