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
    [SerializeField] private float timeSeconds;

    public bool interacted;
    
    public float normalizedHealth = 0.0f;
    public Image state;
    [SerializeField] private GettingOutOfSafeSpace trigger;
    [SerializeField] private Camera focusCamera;
    [SerializeField] private float distanceDecrease;
    
    public OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    private void Start()
    {
        normalizedHealth = currentAnxiety / maxAnxiety;
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
        IncreaseAnxiety();
        focusCamera.focusDistance = 3.3f;
    }
    
    private void ModifyAnxiety(float modifier)
    {
        currentAnxiety = Mathf.Clamp(currentAnxiety + modifier, 0.0f, maxAnxiety);
        normalizedHealth = currentAnxiety / maxAnxiety;
    }
    
    private void EffectsFromAnxiety(float anxietyAmount)
    {
        ModifyAnxiety(anxietyAmount);
        normalizedHealth = currentAnxiety / maxAnxiety;
        
        OnPlayerHealthChangedEvent.Invoke(normalizedHealth);
    }
    
    private void ModifyFocusDistance(float modifier)
    {
        //focusCamera.focusDistance = Mathf.Clamp(focusCamera.focusDistance - modifier, 3.3f, 0f);
    }

    private void VisionEffectsFromAnxiety(float distanceAmount)
    {
        //ModifyFocusDistance(distanceAmount);
    }
    
    private IEnumerator AnxietyLevelsUp()
    {
        while (true)
        {
            if (interacted == false)
            {
                yield return new WaitForSeconds(timeSeconds);
                EffectsFromAnxiety(anxietyIncrease);
            }
            else
            {
                yield return new WaitForSeconds(timeSeconds);
                EffectsFromAnxiety(-anxietyIncrease);
            }

            if (currentAnxiety > maxAnxiety * 0.75f)
            {
                state.color = Color.red;
                //VisionEffectsFromAnxiety(distanceDecrease);
            }
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
