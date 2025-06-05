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
    [SerializeField] private GameManager gameManager; 
    
    [field: Header("Post-Processing Settings")]
    public DepthOfField dOF;
    public ChromaticAberration cA;
    public Vignette vignette;
    
    [field: SerializeField] public float distanceIncrease { get; set; }
    [field: SerializeField] public float distanceDecrease { get; set; }
    [field: SerializeField] public float aberrationIncrease { get; set; }
    [field: SerializeField] public float aberrationDecrease { get; set; }
    
    [field: SerializeField] public float vignetteIncrease { get; set; }
    [field: SerializeField] public float vignetteDecrease { get; set; }
    
    
    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;
    
    [field: SerializeField] public Image state { get; set; }
    
    public OnPlayerHealthChanged OnPlayerHealthChangedEvent;
    
    private void Start()
    {
        //post-processing effects
        dOF = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>();
        cA = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>();
        vignette = focusCamera.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>();
        
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
            if (gameManager.state != IGameStates.Paused)
            {
                if (interacted == false && realityMode == false)
                {
                    EffectsFromAnxiety(anxietyIncrease);
                    if (currentAnxiety <= maxAnxiety * 0.33f) //first third of the anxiety
                    {
                        cA.intensity.value += aberrationIncrease;
                        cA.intensity.value = Mathf.Clamp(cA.intensity.value, 0.2f, 0.8f);
                    }
                
                    if (currentAnxiety > maxAnxiety * 0.33f && currentAnxiety <= maxAnxiety) //starts after the chromatic aberration and ends at the max anxiety
                    {
                        vignette.intensity.value += vignetteIncrease;
                        vignette.intensity.value = Mathf.Clamp(vignette.intensity.value,0f, 0.8f );
                    
                        vignette.smoothness.value += vignetteIncrease;
                        vignette.smoothness.value = Mathf.Clamp(vignette.smoothness.value,0.01f,1f);
                    }
                
                    if (currentAnxiety > maxAnxiety * 0.66f) //final third of the anxiety
                    {
                        dOF.focusDistance.value -= distanceDecrease;
                        dOF.focusDistance.value = Mathf.Clamp01(dOF.focusDistance.value);
                    
                        if (!audioManager.heartbeat2.isPlaying)
                        {
                            audioManager.heartbeat.Stop();
                            audioManager.heartbeat2.Play();
                        }
                    }
                
                    else if (coroutineRunning)
                    {
                        if (!audioManager.heartbeat.isPlaying)
                        {
                            audioManager.heartbeat2.Stop();
                            audioManager.heartbeat.Play();
                        }
                    }
                }
                else if (interacted)
                {
                    EffectsFromAnxiety(-anxietyDecrease);
                    if (!cA.intensity.value.Equals(0.2f))
                    {
                        dOF.focusDistance.value += distanceIncrease;
                        dOF.focusDistance.value = Mathf.Clamp01(dOF.focusDistance.value);
                        
                        cA.intensity.value -= aberrationDecrease;
                        cA.intensity.value = Mathf.Clamp(cA.intensity.value, 0.2f, 0.8f);
                        
                        vignette.intensity.value -= vignetteDecrease;
                        vignette.intensity.value = Mathf.Clamp(vignette.intensity.value,0f, 0.8f );
                        
                        vignette.smoothness.value -= vignetteDecrease;
                        vignette.smoothness.value = Mathf.Clamp(vignette.smoothness.value,0.01f,1f);
                    }

                    if (currentAnxiety <= 0f)
                    {
                        interacted = false;
                        coroutineRunning = false;
                        if (audioManager.heartbeat.isPlaying)
                            audioManager.heartbeat.Stop();
                    }
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
