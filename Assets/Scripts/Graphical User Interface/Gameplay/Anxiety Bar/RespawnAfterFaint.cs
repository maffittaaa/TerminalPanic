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
            RespawnInNearestSafeSpace();
            anxietyBar.currentAnxiety = 0f;
            anxietyBar.state.fillAmount = 0f;
            anxietyBar.dOF.focusDistance.value = 3.8f;
            anxietyBar.cA.intensity.value = 0.3f;
            fadeStarted = false;
        }
    }

    private void RespawnInNearestSafeSpace()
    {
        GameObject closestSafeSpace = new GameObject();
        float minimumDistance = float.MaxValue;
        playerPosition = player.transform.position;

        float currentDistance = float.MaxValue;
        foreach (GameObject safeSpace in safeSpaces)
        {
            currentDistance = Magnitude(playerPosition - safeSpace.transform.position);
            if (currentDistance < minimumDistance)
            {
                minimumDistance = currentDistance;
                closestSafeSpace = safeSpace;
            }
        }
        playerPosition = closestSafeSpace.transform.position;
    }

    private float Magnitude(Vector3 vector)
    {
        return Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
    }
}