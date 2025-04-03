using UnityEngine;

public class GettingOutOfSafeSpace : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !anxietyBar.coroutineRunning)
        {
            StartCoroutine(anxietyBar.RealityVsPanicMode());
            anxietyBar.coroutineRunning = true;
            if (!anxietyBar.realityMode && !anxietyBar.coroutineRunning)
                anxietyBar.IncreaseAnxiety();
        }
    } 
}