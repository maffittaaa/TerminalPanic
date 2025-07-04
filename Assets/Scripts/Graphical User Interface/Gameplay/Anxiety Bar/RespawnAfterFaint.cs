using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterFaint : MonoBehaviour
{
    [SerializeField] private List<GameObject> safeSpaces = new List<GameObject>();
    [SerializeField] private PlayerFainting playerFainting;
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject closestSafeSpace;
    [SerializeField] private WorldInteractions interactions;
    [SerializeField] private CluesTrigger hintsTrigger;
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
            interactions.body.SetActive(false);
            anxietyBar.currentAnxiety = 0f;
            anxietyBar.state.fillAmount = 0f;
            anxietyBar.dOF.focusDistance.value = 1f;
            anxietyBar.cA.intensity.value = 0.2f;
            anxietyBar.vignette.intensity.value = 0f;
            anxietyBar.vignette.smoothness.value = 0.01f;
            fadeStarted = false;
        }
    }

    private float Magnitude(Vector3 vector)
    {
        return Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
    }
    
    private void RespawnInNearestSafeSpace()
    {
        float minimumDistance = 10000;

        float currentDistance = 10000;
        foreach (GameObject safeSpace in safeSpaces)
        {
            currentDistance = Magnitude(player.transform.position - safeSpace.transform.position);
            if (currentDistance < minimumDistance)
            {
                minimumDistance = currentDistance;
                closestSafeSpace = safeSpace;
            }
        }
        player.transform.position = closestSafeSpace.transform.position;
    }
}