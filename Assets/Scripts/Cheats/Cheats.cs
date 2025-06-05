using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private ClueText clueText;
    [SerializeField] private LightManager lightManager;
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private TravelerSpawner travelerSpawner;
    [SerializeField] private WorldInteractions killPlayer;
    private bool invulnerable = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            clueText.TextForClue();
        else if (Input.GetKeyDown(KeyCode.L))
            lightManager.TurnOnAndOffManager();
        else if (Input.GetKeyDown(KeyCode.P))
            travelerSpawner.SwitchToPanicMode();
        else if (Input.GetKeyDown(KeyCode.N))
            travelerSpawner.SwitchToNormalMode();
        else if (Input.GetKeyDown(KeyCode.C))
        {
            anxietyBar.currentAnxiety = 0f;
            anxietyBar.dOF.focusDistance.value = 1f;
            anxietyBar.cA.intensity.value = 0.2f;
            anxietyBar.vignette.intensity.value = 0f;
            anxietyBar.vignette.smoothness.value = 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.M))
            anxietyBar.currentAnxiety = anxietyBar.maxAnxiety * 0.75f;
        else if (Input.GetKeyDown(KeyCode.I))
        {
            invulnerable = !invulnerable;
            if (!invulnerable)
                killPlayer.KillPlayer();
        }
    }
}
