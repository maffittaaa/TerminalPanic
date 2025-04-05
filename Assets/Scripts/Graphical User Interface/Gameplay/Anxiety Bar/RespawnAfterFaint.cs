using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterFaint : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private GettingOutOfSafeSpace gettingOutOfSafeSpace;
    [SerializeField] private PlayerFainting playerFainting;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private List<GameObject> safeSpaces = new List<GameObject>();
    private Vector3 playerPosition;
    private bool fadeEnded = true;
    private bool fadeStarted = false;
    
    private void Update()
    {
        if (anxietyBar.currentAnxiety == anxietyBar.maxAnxiety && playerFainting.FadeEnded() == false && fadeStarted == false)
        {
            fadeStarted = true;
            playerFainting.ResetAndStartFade();
            fadeEnded = false;
        }
        
        if (fadeEnded == false && playerFainting.FadeEnded() && fadeStarted)
        {
            fadeEnded = true;
            anxietyBar.respawningAfterFaint = true;
            anxietyBar.ResetAnxiety();
            this.transform.position = new Vector3(27.53f,3.79f,-301.08f);
            anxietyBar.currentAnxiety = 0f;
            anxietyBar.state.fillAmount = 0f;
            anxietyBar.dOF.focusDistance.value = 3.8f;
            anxietyBar.cA.intensity.value = 0.3f;
            fadeStarted = false;
        }
    }

    private void RespawnInNearestSafeSpace()
    {
        playerPosition = player.transform.position;
    }
}