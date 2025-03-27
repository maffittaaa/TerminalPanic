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
    public float maxAnxiety;
    public float currentAnxiety = 0.0f;
    [SerializeField] private float anxietyIncrease = 2.0f;
    
    [Header("Increasing Over Time")]
    [SerializeField] private float timeSeconds;

    public bool interacted;
    
    public float normalizedAnxiety = 0.0f;
    public Image state;
    [SerializeField] private GettingOutOfSafeSpace trigger;
    [SerializeField] private PostProcessVolume focusCamera;
    public DepthOfField dOF;
    public ChromaticAberration cA;
    [SerializeField] private float distanceDecrease;
    [SerializeField] private float aberrationIncrease;
    
    public OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    private void Start()
    {
        normalizedAnxiety = currentAnxiety / maxAnxiety;
        OnPlayerHealthChangedEvent.Invoke(normalizedAnxiety);
        IncreaseAnxiety();
        dOF = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>();
        cA = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>();
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
