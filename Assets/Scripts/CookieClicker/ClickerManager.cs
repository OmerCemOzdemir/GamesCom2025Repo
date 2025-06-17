using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClickerManager : MonoBehaviour
{
    [SerializeField]
    private GameRule _gameRule;
    [SerializeField]
    private ClickerUI clickerUI;
    [SerializeField]
    private int Level;

    private bool mouseEnable = false;

    private InputSystem _inputSystem;

    private void OnEnable()
    {
        _inputSystem.PlayerCookie.GetMoney.Enable();
        _inputSystem.PlayerCookie.GetMoney.performed += OnClickIncreaseMoney;
        CheckMousePos.onMouseOver += checkMousePos;
    }
    private void OnDisable()
    {
        _inputSystem.PlayerCookie.GetMoney.Disable();
        _inputSystem.PlayerCookie.GetMoney.performed -= OnClickIncreaseMoney;
        CheckMousePos.onMouseOver -= checkMousePos;
    }
    private void Awake()
    {
      setUpData();
      _inputSystem = new InputSystem();
    }

    public void addItemLvl()
    {
        _gameRule.AddItemLvl(Level);
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
    }
    
    public void OnClickIncreaseMoney(InputAction.CallbackContext context)
    {
        if (mouseEnable)
        {
            _gameRule.IncreaseScore();
            clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
        }
    }
    public void nextScene(int index)
    {
        GameManager.Instance.SaveGame();
        GameManager.Instance.NextLevel(index);
    }
    private void checkMousePos(bool checkMouseEnable)
    {
        mouseEnable = checkMouseEnable;
    }
    private void setUpData()
    {
        GameManager.Instance.GetGameData();
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
        clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
    }
}
