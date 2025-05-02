using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum IGameStates { Running, Paused, MainMenu }
public enum IUIType { Button, Other}

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
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private Slider brightnessSlider;

    [Header("Game State")]
    [SerializeField] private GameManager manager;
    [SerializeField] private IUIType type;
    [field: SerializeField] public IGameStates state {  get; private set; }

    void Start()
    {
        state = IGameStates.Running;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void PlayModeView()
    {
        pauseView.SetActive(false);
        optionsView.SetActive(false);
        settingsView.SetActive(false);
        NonHouveredColor();

        if(IUIType.Button == type)
        {
            manager.state = IGameStates.Running;
        }
        else
        {
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
            manager.state = IGameStates.Paused;
        }
        else
        {
            state = IGameStates.Paused;
        }
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

    private void Update()
    {
        if(type != IUIType.Button)
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
}
