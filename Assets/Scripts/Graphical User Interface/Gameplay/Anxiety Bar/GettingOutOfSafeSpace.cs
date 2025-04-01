using UnityEngine;

public class GettingOutOfSafeSpace : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (anxietyBar.coroutineRunning == false)
            {
                anxietyBar.interacted = false;
                anxietyBar.coroutineRunning = true;
                anxietyBar.IncreaseAnxiety();
            }
        }
    } 
}