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
        Debug.Log("Motherfucker2!");
        normalizedHealth = currentAnxiety / maxAnxiety;
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
    }
    
    public void ModifyHealth(float modifier)
    {
        Debug.Log("Motherfucker!");
        currentAnxiety = Mathf.Clamp(currentAnxiety + modifier, 0.0f, maxAnxiety);
        normalizedHealth = currentAnxiety / maxAnxiety;
    }
    
    public void EffectsFromAnxiety(float anxietyAmount) //damage from radiation if player doesn't take an anti-radiation flask
    {
        Debug.Log("Hi!");
        ModifyHealth(anxietyAmount);
        StartCoroutine(AnxietyOverTime());
        normalizedHealth = currentAnxiety / maxAnxiety;
        
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
    }

    private IEnumerator AnxietyOverTime()
    {
        Debug.Log("Hello!");
        yield return new WaitForSeconds(anxietyTimeSeconds);
        AnxietyLevelsUp();
    }
    
    public void AnxietyLevelsUp()
    {
        if (interacted == false)
            EffectsFromAnxiety(anxietyIncrease);
        else
            interacted = true;
    }
}
