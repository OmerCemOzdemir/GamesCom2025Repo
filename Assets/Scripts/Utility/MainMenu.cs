using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        GameManager.Instance.GetGameData();
    }

    public void StartGame()
    {
        GameManager.Instance.NextLevel(1);
    }


}
