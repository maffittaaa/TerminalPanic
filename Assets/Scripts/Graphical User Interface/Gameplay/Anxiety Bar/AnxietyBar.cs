using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnPlayerHealthChanged : UnityEvent<float> { };
public class AnxietyBar : MonoBehaviour
{
    [Header("Anxiety")]
    [SerializeField] private float maxAnxiety = 200.0f;
    [SerializeField] private float currentAnxiety = 0.0f;
    [SerializeField] private float anxietyIncrease = 2.0f;
    
    [Header("Increasing Over Time")]
    [SerializeField] private float anxietyTimeSeconds;

    private bool interacted = false;
    
    private float normalizedHealth = 0.0f;
    
    public OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        normalizedHealth = currentAnxiety / maxAnxiety;
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
    }
    
    public void ModifyHealth(float modifier)
    {
        currentAnxiety = Mathf.Clamp(currentAnxiety + modifier, 0.0f, maxAnxiety);
        normalizedHealth = currentAnxiety / maxAnxiety;
    }
    
    public void EffectsFromAnxiety(float anxietyAmount)
    {
        ModifyHealth(anxietyAmount);
        normalizedHealth = currentAnxiety / maxAnxiety;
        
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
    }
    
    public IEnumerator AnxietyLevelsUp()
    {
        if (interacted == false)
        {
            yield return new WaitForSeconds(anxietyTimeSeconds);
            EffectsFromAnxiety(anxietyIncrease);
        }
        else
            interacted = true;
    }

    private void Update()
    {
        StartCoroutine(AnxietyLevelsUp());
        Debug.Log(currentAnxiety);
    }
}
