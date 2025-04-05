using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[System.Serializable]
public class OnPlayerHealthChanged : UnityEvent<float> { };
public class AnxietyBar : MonoBehaviour
{
    [field: Header("Anxiety")] [field: SerializeField]
    public float maxAnxiety = 200f;
    public float currentAnxiety = 0.0f;
    [SerializeField] private float anxietyIncrease = 0.5f;
    [SerializeField] private float anxietyDecrease = 1.5f;
    private float normalizedAnxiety = 0.0f;
    
    [field: Header("Time")]
    [SerializeField] private float timeSeconds;
    [SerializeField] private float timeOnReality;
    
    [field: Header("Booleans")]
    [field: SerializeField] public bool interacted  { get; set; }
    [field: SerializeField] public bool coroutineRunning { get; set; }
    [field: SerializeField] public bool realityMode { get; set; }
    [field: SerializeField] public bool respawningAfterFaint { get; set; }

    [field: Header("Scripts")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private LightManager flickeringLights;
    [SerializeField] private GettingOutOfSafeSpace trigger;
    [SerializeField] private PostProcessVolume focusCamera;
    
    [field: Header("Post-Processing Settings")]
    public DepthOfField dOF;
    public ChromaticAberration cA;
    [field: SerializeField] public float distanceDecrease { get; set; }
    [field: SerializeField] public float aberrationIncrease { get; set; }
    
    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    
    [field: SerializeField] public Image state { get; set; }
    
    public OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    
    private void Start()
    {
        dOF = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>();
        cA = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>();
        
        normalizedAnxiety = currentAnxiety / maxAnxiety;
        OnPlayerHealthChangedEvent.Invoke(normalizedAnxiety);
        
        coroutineRunning = false;
        realityMode = false;
        respawningAfterFaint = false;
        
        IncreaseAnxiety();
    }
    
    private void FixedUpdate()
    {
        if (audioManager == null)
            audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
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
            yield return new WaitForSeconds(timeSeconds);
            if (interacted == false && realityMode == false)
            { 
                EffectsFromAnxiety(anxietyIncrease);
            }
            else if (interacted)
            {
                EffectsFromAnxiety(-anxietyDecrease);
                if (!dOF.focusDistance.value.Equals(1f) && !cA.intensity.value.Equals(1f))
                {
                    dOF.focusDistance.value += distanceDecrease;
                    cA.intensity.value -= aberrationIncrease;
                }
                
                if (currentAnxiety <= 0f)
                {
                    interacted = false;
                    coroutineRunning = false;
                    if (audioManager.heartbeat.isPlaying)
                        audioManager.heartbeat.Stop();
                }
            }

            if (currentAnxiety > maxAnxiety * 0.75f)
            {
                state.color = Color.red;
                dOF.focusDistance.value -= distanceDecrease;
                cA.intensity.value += aberrationIncrease;
                if (!audioManager.heartbeat2.isPlaying)
                {
                    audioManager.heartbeat.Stop();
                    audioManager.heartbeat2.Play();
                }
            }
            else if (coroutineRunning)
            {
                state.color = Color.yellow;
                if (!audioManager.heartbeat.isPlaying)
                {
                    audioManager.heartbeat2.Stop();
                    audioManager.heartbeat.Play();
                }
            }
        }
    }

    public IEnumerator FreezeMovementWhileCalming()
    {
        playerMovement.speed = 0f;
        yield return new WaitUntil(CurrentAnxietyIsZero);
        flickeringLights.TurnOnAndOffManager();
    }

    public IEnumerator RealityVsPanicMode()
    {
        yield return new WaitForSeconds(timeOnReality);
        realityMode = false;
        coroutineRunning = true;
        flickeringLights.TurnOnAndOffManager();
    }

    private bool CurrentAnxietyIsZero()
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
        audioManager.heartbeat.Stop();
        audioManager.heartbeat2.Stop();
        StopAllCoroutines();
    }
}
