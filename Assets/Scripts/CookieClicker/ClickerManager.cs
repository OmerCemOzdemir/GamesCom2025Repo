using System;
using UnityEngine;
using UnityEngine.UI;

public class ClickerManager : MonoBehaviour
{
    [SerializeField]
    private GameRule _gameRule;
    [SerializeField]
    private ClickerUI clickerUI;

    [SerializeField]
    private int Level;

    private void Awake()
    {
      setUpData();
    }

    public void addItemLvl()
    {
        _gameRule.AddItemLvl(Level);
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
    }
    
    public void OnClickIncreaseMoney()
    {
        _gameRule.IncreaseScore();
        clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
    }
    public void nextScene(int index)
    {
        GameManager.Instance.SaveGame();
        GameManager.Instance.NextLevel(index);
    }

    private void setUpData()
    {
        GameManager.Instance.GetGameData();
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
        clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
    }
}
