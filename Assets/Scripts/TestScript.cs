using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI printText;

    private void Awake()
    {
        totalMoneyText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        totalMoneyText.text = "" + GameManager.Instance.GetGameData().totalMoney;
    }

    public void IncreaseMoney()
    {
        GameManager.Instance.GetGameData().totalMoney += 1;
        totalMoneyText.text = "" + GameManager.Instance.GetGameData().totalMoney;
        //Debug.Log("Total Money: " + GameManager.Instance.getGameData().totalMoney);

    }

    public void DecreaseMoney()
    {
        if (GameManager.Instance.GetGameData().totalMoney > 0)
        {
            GameManager.Instance.GetGameData().totalMoney -= 10;
        }
        totalMoneyText.text = "" + GameManager.Instance.GetGameData().totalMoney;
    }

    public void GoNextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        GameManager.Instance.NextLevel(level);
    }

    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();

    }

    public void PrintGameData()
    {
        printText.text = "Total Money: " + GameManager.Instance.GetGameData().totalMoney + "\n"
            + "New Game: " + GameManager.Instance.GetGameData().newGame; 

    }

}
