using System;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;


    private void Awake()
    {
        GameManager.Instance.GetGameData();

        if (!GameManager.Instance.GetGameData().startNewGame)
        {
            continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
            newGameButton.SetActive(true);
        }

    }

    public void NewGame()
    {
        confirmPanel.SetActive(true);
    }

    public void ConfirmNewGame()
    {
        GameManager.Instance.ResetGameData();
        GameManager.Instance.NextLevel(7);
    }

    public void DenyNewGame()
    {
        confirmPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (GameManager.Instance.GetGameData().startNewGame)
        {
            GameManager.Instance.ResetGameData();
            GameManager.Instance.NextLevel(7);
            GameManager.Instance.GetGameData().startNewGame = false;
            GameManager.Instance.SaveGame();
        }
        else
        {
            GameManager.Instance.NextLevel(1);
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}


/*
         //onPlaySFX?.Invoke(SFX.Generic);

    public static event Action<Music> onPlayMusic;
    public static event Action<SFX> onPlaySFX;
 */