using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class OnPlayerHealthChanged : UnityEvent<float> { };
public class AnxietyBar : MonoBehaviour
{
    [Header("Anxiety")]
    public float maxAnxiety = 200.0f;
    public float currentAnxiety = 0.0f;
    [SerializeField] private float anxietyIncrease = 2.0f;
    
    [Header("Increasing Over Time")]
    [SerializeField] private float anxietyTimeSeconds;

    public bool interacted;
    
    public float normalizedHealth = 0.0f;
    public Image state;
    public bool isCoroutineRunning = false;
    [SerializeField] private GettingOutOfSafeSpace trigger;
    
    public OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    private void Start()
    {
        normalizedHealth = currentAnxiety / maxAnxiety;
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
        IncreaseAnxiety(); 
    }
    
    private void ModifyHealth(float modifier)
    {
        currentAnxiety = Mathf.Clamp(currentAnxiety + modifier, 0.0f, maxAnxiety);
        normalizedHealth = currentAnxiety / maxAnxiety;
    }
    
    private void EffectsFromAnxiety(float anxietyAmount)
    {
        ModifyHealth(anxietyAmount);
        normalizedHealth = currentAnxiety / maxAnxiety;
        
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
    }
    
    private IEnumerator AnxietyLevelsUp()
    {
        while (true)
        {
            if (interacted == false)
            {
                yield return new WaitForSeconds(anxietyTimeSeconds);
                EffectsFromAnxiety(anxietyIncrease);
            }
            else
            {
                yield return new WaitForSeconds(anxietyTimeSeconds);
                EffectsFromAnxiety(-anxietyIncrease);
            }
            
            if (currentAnxiety > maxAnxiety * 0.75f)
                state.color = Color.red;
            else
                state.color = Color.yellow;
        }
    }

    public void IncreaseAnxiety()
    {
        StartCoroutine(AnxietyLevelsUp());
    }
    
    public void ResetAnxiety()
    {
        StopAllCoroutines();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            interacted = true;
    }
}
