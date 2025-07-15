using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static event Action<Music> onPlayMusic;
    public static event Action<SFX> onPlaySFX;

    private void Awake()
    {
        GameManager.Instance.GetGameData();
    }

    private void Start()
    {
        onPlayMusic?.Invoke(Music.MainMenu);
    }


    public void StartGame()
    {
        onPlaySFX?.Invoke(SFX.Generic);
        GameManager.Instance.NextLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
