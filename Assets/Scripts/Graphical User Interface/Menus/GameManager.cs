using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Profiling;
using UnityEngine.Audio;

public enum IGameStates { Running, Paused, MainMenu }
public enum IUIType { Button, Slider, GameManager}

public class GameManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Views")]
    [SerializeField] private GameObject pauseView;
    [SerializeField] private GameObject optionsView;
    [SerializeField] private GameObject settingsView;

    [Header("Color Buttons")]
    [SerializeField] private Color houveColor;

    [Header("Settings Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider brightnessSlider;

    [Header("Post Processing Effects")]
    [SerializeField] private PostProcessVolume postProcess;

    //Depth
    private DepthOfField depth;
    [SerializeField] private float newDepthValue;
    [SerializeField] private float currentDepthValue;
    
    //Bloom
    private Bloom bloom;
    private float currentBloomValue;
    [SerializeField] private float newBloomValue;

    //Chromatic Aberration
    private ChromaticAberration chromaticAberration;
    private float currentChromaticAberrationValue;
    [SerializeField] private float newChromaticAberrationValue;

    //Auto Exposure
    private AutoExposure autoExposure;
    [SerializeField] private float newAutoExposureValue;

    [Header("Game State")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private IUIType type;
    [field: SerializeField] public IGameStates state {  get; private set; }



    void Start()
    {
        state = IGameStates.Running;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (IUIType.GameManager == type)
        {
            SetPostProcessEffects();
            InitializeAudio();
        }
    }

    private void InitializeAudio()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.SetMasterVolume(masterSlider.value);
        audioManager.SetMusicVolume(SFXSlider.value);
        audioManager.SetSFXVolume(musicSlider.value);
    }

    private void SetPostProcessEffects()
    {
        postProcess = GameObject.FindGameObjectWithTag("PostProcessEffects").GetComponent<PostProcessVolume>();
        depth = postProcess.profile.GetSetting<DepthOfField>();
        bloom = postProcess.profile.GetSetting<Bloom>();
        chromaticAberration = postProcess.profile.GetSetting<ChromaticAberration>();
        autoExposure = postProcess.profile.GetSetting<AutoExposure>();

        depth.focalLength.overrideState = false;

        currentBloomValue = bloom.intensity.value;
        currentChromaticAberrationValue = chromaticAberration.intensity.value;
        currentDepthValue = depth.focusDistance.value;

        PlayModeView();
    }

    public void PlayModeView()
    {
        pauseView.SetActive(false);
        optionsView.SetActive(false);
        settingsView.SetActive(false);
        NonHouveredColor();
        if (IUIType.Button == type)
        {
            gameManager.RestorePostProcessEffects();
            gameManager.state = IGameStates.Running;
        }
        else if (IUIType.GameManager == type)
        {
            RestorePostProcessEffects();
            state = IGameStates.Running;
        }
    }

    public void PauseMenuView()
    {
        pauseView.SetActive(true);
        optionsView.SetActive(true);
        settingsView.SetActive(false);
        NonHouveredColor();
        if (IUIType.Button == type)
        {
            gameManager.ChangePostProcessEffects();
            gameManager.state = IGameStates.Paused;
            gameManager.StopAllSounds();
        }
        else if (IUIType.GameManager == type)
        {
            ChangePostProcessEffects();
            state = IGameStates.Paused;
            StopAllSounds();
        }
    }

    public void BackButtonSettings()
    {
        pauseView.SetActive(true);
        optionsView.SetActive(true);
        settingsView.SetActive(false);
        NonHouveredColor();
    }

    public void SettingsView()
    {
        pauseView.SetActive(true);
        optionsView.SetActive(false);
        settingsView.SetActive(true);
        NonHouveredColor();
    }

    public void GoToMainMenu()
    {
        //load main menu scene when its created
        Application.Quit();
    }

    public void HouveredColor()
    {
        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = houveColor;
    }

    public void NonHouveredColor()
    {
        if (type == IUIType.Button)
        {
            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(type == IUIType.Button)
        {
            HouveredColor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (type == IUIType.Button)
        {
            NonHouveredColor();
        }
    }

    public void ChangeAutoExposure()
    {
        autoExposure.keyValue.value = brightnessSlider.value;
    }

    private void ChangePostProcessEffects()
    {
        currentBloomValue = bloom.intensity.value;
        currentChromaticAberrationValue = chromaticAberration.intensity.value;
        currentDepthValue = depth.focusDistance.value;

        depth.focalLength.overrideState = true;
        depth.focusDistance.value = 1;
        depth.focalLength.value = newDepthValue;
        bloom.intensity.value = newBloomValue;
        chromaticAberration.intensity.value = newChromaticAberrationValue;
    }

    public void RestorePostProcessEffects()
    {
        depth.focalLength.overrideState = false;

        depth.focusDistance.value = currentDepthValue;
        bloom.intensity.value = currentBloomValue;
        chromaticAberration.intensity.value = currentChromaticAberrationValue;
    }

    private void Update()
    {
        if (IUIType.GameManager == type)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (state == IGameStates.Running)
                {
                    PauseMenuView();
                }
                else if (state == IGameStates.Paused)
                {
                    PlayModeView();
                }
            }
           
            SetMouseSettings(state == IGameStates.Running);
        }
    }

    private void SetMouseSettings(bool state)
    {
        if (state)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void StopAllSounds()
    {
        foreach(AudioSource audio in audioManager.allAudios)
        {
            if (audio)
            {
                audio.Stop();
            }
        }
    }
}
