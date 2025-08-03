using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private InputSystem GUI_InputAction;
    private bool togglePauseGame = true;

    private void Awake()
    {
        GUI_InputAction = new InputSystem();
    }

    private void Start()
    {
        if (bgmSlider != null)
        {
            bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume", 0.5f);
            bgmSlider.onValueChanged.AddListener((v) =>
            {
                AudioManager.Instance.SetBGMVolume(v);
                PlayerPrefs.SetFloat("bgmVolume", v);
            });
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
            sfxSlider.onValueChanged.AddListener((v) =>
            {
                AudioManager.Instance.SetSFXVolume(v);
                PlayerPrefs.SetFloat("sfxVolume", v);
            });
        }
    }
    
    private void OnEnable()
    {

        GUI_InputAction.GUI.Pause.Enable();
        GUI_InputAction.GUI.Pause.performed += PauseGame;

    }

    private void OnDisable()
    {
        GUI_InputAction.GUI.Pause.Disable();
        GUI_InputAction.GUI.Pause.performed -= PauseGame;

    }

    public void Return()
    {
        debugPanel.SetActive(false);
        settingPanel.SetActive(false);
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        if (togglePauseGame)
        {
            pauseMenu.SetActive(true);
            togglePauseGame = false;
        }
        else
        {
            pauseMenu.SetActive(false);
            togglePauseGame = true;
        }
    }

    public void BackToOffice()
    {
        GameManager.Instance.NextLevel(1);
    }

    public void Resume()
    {
        if (togglePauseGame)
        {
            pauseMenu.SetActive(true);
            togglePauseGame = false;
        }
        else
        {
            pauseMenu.SetActive(false);
            togglePauseGame = true;
        }
    }

    public void OpenSettings()
    {
        settingPanel.SetActive(true);
    }

    public void OpenDebugPanel()
    {
        debugPanel.SetActive(true);
    }

    public void Quit()
    {
        GameManager.Instance.SaveGame();
        Application.Quit();
    }

    public void CloseAllPanels()
    {
        settingPanel.SetActive(false);
        debugPanel.SetActive(false);
    }

}
