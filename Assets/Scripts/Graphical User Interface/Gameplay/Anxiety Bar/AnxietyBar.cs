using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[System.Serializable]
public class OnPlayerHealthChanged : UnityEvent<float> { };
public class AnxietyBar : MonoBehaviour
{
    [Header("Anxiety")]
    public float maxAnxiety = 200f;
    public float currentAnxiety = 0.0f;
    [SerializeField] private float anxietyIncrease = 0.5f;
    public float normalizedAnxiety = 0.0f;
    
    [Header("Increasing Over Time")]
    [SerializeField] private float timeSeconds;

    public bool interacted;
    public bool coroutineRunning;

    [SerializeField] private PlayerMovement playerMovement;
    
    public Image state;
    [SerializeField] private GettingOutOfSafeSpace trigger;
    [SerializeField] private PostProcessVolume focusCamera;
    public DepthOfField dOF;
    public ChromaticAberration cA;
    [SerializeField] private float distanceDecrease = 0.03f;
    [SerializeField] private float aberrationIncrease = 0.03f;
    
    public OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    private void Start()
    {
        dOF = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>();
        cA = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>();
        
        normalizedAnxiety = currentAnxiety / maxAnxiety;
        OnPlayerHealthChangedEvent.Invoke(normalizedAnxiety);
        
        coroutineRunning = false;
        IncreaseAnxiety();
    }
    
    private void ModifyAnxiety(float modifier)
    {
        currentAnxiety = Mathf.Clamp(currentAnxiety + modifier, 0.0f, maxAnxiety);
        normalizedAnxiety = currentAnxiety / maxAnxiety;
    }
    
    private void EffectsFromAnxiety(float anxietyAmount)
    {
        ModifyAnxiety(anxietyAmount);
        normalizedAnxiety = currentAnxiety / maxAnxiety;
        
        OnPlayerHealthChangedEvent.Invoke(normalizedAnxiety);
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
                if (currentAnxiety == 0f)
                    coroutineRunning = false;
            }

            if (currentAnxiety > maxAnxiety * 0.75f)
            {
                state.color = Color.red;
                yield return new WaitForSeconds(timeSeconds);
                dOF.focusDistance.value -= distanceDecrease;
                cA.intensity.value += aberrationIncrease;
            }
            else
                state.color = Color.yellow;
        }
    }

    public IEnumerator FreezeMovementWhileCalming()
    {
        playerMovement.speed = 0f;
        yield return new WaitUntil(currentAnxietyIsZero);
        playerMovement.speed = playerMovement.initialSpeed;
    }

    private bool currentAnxietyIsZero()
    {
        return currentAnxiety <= 0;
    }

    public void IncreaseAnxiety()
    {
        coroutineRunning = true;
        StartCoroutine(AnxietyLevelsUp());
    }
    
    public void ResetAnxiety()
    {
        coroutineRunning = false;
        StopAllCoroutines();
    }
}
