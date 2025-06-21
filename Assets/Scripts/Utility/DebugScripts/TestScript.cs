using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI printText;


    private void Start()
    {

        //totalMoneyText.text = "" + GameManager.Instance.GetGameData().totalMoney;
    }

    public void IncreaseMoney()
    {
        GameManager.Instance.GetGameData().totalMoney += 100;
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

    public void ResetGameData()
    {
        GameManager.Instance.ResetGameData();

    }

    public void ResetMoneyData()
    {
        GameManager.Instance.GetGameData().totalMoney = 0;
        GameManager.Instance.SaveGame();
        GameManager.Instance.LoadGame();
    }

    public void PrintGameData()
    {
        printText.text = "Total Money: " + GameManager.Instance.GetGameData().totalMoney + "\n"
            + "Platform Game Var:\n"
            + "Item Jump Boots: " + GameManager.Instance.GetGameData().platformItems[0] + "\n"
            + "Item Sprint Boots: " + GameManager.Instance.GetGameData().platformItems[1] + "\n"
            + "Item Spring Soles: " + GameManager.Instance.GetGameData().platformItems[2] + "\n"
            + "Item Climb Gloves: " + GameManager.Instance.GetGameData().platformItems[3] + "\n"
            + "Clicker Game Var:\n"
            + "ItemBaseIncome: " + GameManager.Instance.GetGameData().ItemBaseIncome + "\n"
            + "ItemCount: " + GameManager.Instance.GetGameData().ItemCount + "\n"
            + "ItemMulti: " + GameManager.Instance.GetGameData().ItemMulti;
    }

}

/*
     //Data For only Cookie Clicker Game:
    public float ItemBaseIncome = 1.67f;
    public int ItemCount = 1;
    public int ItemMulti = 2;
 
 */