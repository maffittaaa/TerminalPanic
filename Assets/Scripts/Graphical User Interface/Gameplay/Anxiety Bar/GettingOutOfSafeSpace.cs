using UnityEngine;

public class GettingOutOfSafeSpace : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && anxietyBar.coroutineRunning == false)
        {
            Debug.Log("Getting out of safe space");
            StartCoroutine(anxietyBar.RealityVsPanicMode());
            anxietyBar.coroutineRunning = true;
            if (!anxietyBar.realityMode)
                anxietyBar.IncreaseAnxiety();
        }
    } 
}