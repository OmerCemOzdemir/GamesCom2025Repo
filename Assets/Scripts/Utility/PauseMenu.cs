using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject debugPanel;

    private InputSystem GUI_InputAction;
    private bool togglePauseGame = true;

    private void Awake()
    {
        GUI_InputAction = new InputSystem();
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
