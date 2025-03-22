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
        if (Input.GetKeyDown(KeyCode.C) && anxietyBar.interacted == false && gettingOutOfSafeSpace.Collided(true))
            anxietyBar.AnxietyLevelsUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.transform.position = new Vector3(27.53f,3.79f,-301.08f);
            anxietyBar.state.fillAmount = 0f;
            anxietyBar.currentAnxiety = 0f;
            anxietyBar.StopCoroutine(anxietyBar.AnxietyLevelsUp());
        }
    }
}
