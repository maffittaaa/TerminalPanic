using UnityEngine;

public class GettingOutOfSafeSpace : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !anxietyBar.coroutineRunning)
        {
            if (anxietyBar.realityMode) // when reality mode is on
            {
                StartCoroutine(anxietyBar.RealityVsPanicMode());
                anxietyBar.coroutineRunning = true;
            }
            
            if (anxietyBar.respawningAfterFaint) //for when the player faints, respawns and leaves safe space
            {
                anxietyBar.IncreaseAnxiety();
                anxietyBar.respawningAfterFaint = false;
            }
        }
    } 
}