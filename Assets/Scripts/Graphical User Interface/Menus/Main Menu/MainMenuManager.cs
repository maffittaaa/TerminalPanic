using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("Panels")]
    public GameObject settingsPanel;

    [Header("Settings Options")]
    public Button keyboardControlsButton;
    public Image keyboardControlsImage;
    public List<Selectable> settingsOptions;

    [Header("Quit Confirmation")]
    public GameObject quitConfirmationPanel;
    public Button yesButton;
    public Button noButton;

    public GameObject dimmerPanel;

    private List<Button> mainButtons;
    private bool keyboardControlsActive = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mainButtons = new List<Button> { startButton, settingsButton, quitButton };

        startButton.onClick.AddListener(() => OnMainButtonClicked(startButton, StartGame));
        settingsButton.onClick.AddListener(() => OnMainButtonClicked(settingsButton, ToggleSettings));
        quitButton.onClick.AddListener(() => OnMainButtonClicked(quitButton, QuitGame));

        settingsPanel.SetActive(false);
        keyboardControlsImage.gameObject.SetActive(false);
        keyboardControlsButton.onClick.AddListener(ToggleKeyboardControls);

        quitConfirmationPanel.SetActive(false);

        yesButton.onClick.AddListener(QuitGameConfirmed);
        noButton.onClick.AddListener(CancelQuit);
    }

    void OnMainButtonClicked(Button clicked, System.Action action)
    {
        HighlightMainButton(clicked, mainButtons);
        action.Invoke();
    }

    void HighlightMainButton(Button active, List<Button> buttons)
    {
        foreach (var btn in buttons)
        {
            var cg = btn.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = btn.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = (active == null || btn == active) ? 1f : 0.4f;
            cg.interactable = true;
        }
    }


    void HighlightButton(Selectable active, List<Selectable> options)
    {
        foreach (var opt in options)
        {
            var cg = opt.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = opt.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = (active == null || opt == active) ? 1f : 0.4f;
            cg.interactable = true;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ToggleSettings()
    {
        bool isActive = settingsPanel.activeSelf;
        settingsPanel.SetActive(!isActive);

        if (!isActive)
        {
            dimmerPanel.SetActive(true);
            HighlightButton(null, settingsOptions); 
        }
        else
        {
            settingsPanel.SetActive(false);
            dimmerPanel.SetActive(false);
            HighlightMainButton(null, mainButtons);
            if (keyboardControlsActive)
            {
                keyboardControlsImage.gameObject.SetActive(false);
                keyboardControlsActive = false;

                foreach (var opt in settingsOptions)
                {
                    if (opt != keyboardControlsButton)
                    {
                        var cg = opt.GetComponent<CanvasGroup>();
                        if (cg == null)
                            cg = opt.gameObject.AddComponent<CanvasGroup>();

                        cg.alpha = 1f;
                        cg.interactable = true;
                    }
                }
            }
        }
    }

    public void ToggleKeyboardControls()
    {
        keyboardControlsActive = !keyboardControlsActive;
        keyboardControlsImage.gameObject.SetActive(keyboardControlsActive);

        foreach (var option in settingsOptions)
        {
            if (option != keyboardControlsButton)
            {
                var cg = option.GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = option.gameObject.AddComponent<CanvasGroup>();

                cg.alpha = keyboardControlsActive ? 0.4f : 1f;
                cg.interactable = !keyboardControlsActive;
            }
        }

        var kbcg = keyboardControlsButton.GetComponent<CanvasGroup>();
        if (kbcg == null)
            kbcg = keyboardControlsButton.gameObject.AddComponent<CanvasGroup>();

        kbcg.alpha = 1f;
    }

    public void QuitGame()
    {
        quitConfirmationPanel.SetActive(true);
        dimmerPanel.SetActive(true);

        foreach (var btn in mainButtons)
        {
            var cg = btn.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = btn.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 0.4f;
            cg.interactable = false;
        }

        if (settingsPanel.activeSelf)
        {
            CloseSettings();
        }
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false);
        dimmerPanel.SetActive(false);

        if (keyboardControlsActive)
        {
            keyboardControlsImage.gameObject.SetActive(false);
            keyboardControlsActive = false;

            foreach (var opt in settingsOptions)
            {
                if (opt != keyboardControlsButton)
                {
                    var cg = opt.GetComponent<CanvasGroup>();
                    if (cg == null)
                        cg = opt.gameObject.AddComponent<CanvasGroup>();

                    cg.alpha = 1f;
                    cg.interactable = true;
                }
            }
        }

        foreach (var btn in mainButtons)
        {
            var cg = btn.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = btn.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 1f;
            cg.interactable = true;
        }
    }


    void QuitGameConfirmed()
    {
        Application.Quit();
    }

    void CancelQuit()
    {
        quitConfirmationPanel.SetActive(false);
        dimmerPanel.SetActive(false);

        foreach (var btn in mainButtons)
        {
            var cg = btn.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = btn.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 1f;
            cg.interactable = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else if (quitConfirmationPanel.activeSelf)
            {
                CancelQuit();
            }
        }
    }

}
