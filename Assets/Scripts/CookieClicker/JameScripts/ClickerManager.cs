using UnityEngine;
using UnityEngine.InputSystem;

public class ClickerManager : MonoBehaviour
{
    [SerializeField] private GameRule _gameRule;
    [SerializeField] private ClickerUI clickerUI;
    [SerializeField] private int Level;

    [SerializeField] private animationManager anim;

    private bool mouseEnable = false;
    private InputSystem _inputSystem;

    private void OnEnable()
    {
        _inputSystem.PlayerCookie.GetMoney.Enable();
        _inputSystem.PlayerCookie.GetMoney.performed += OnClickIncreaseMoney;
        global::CheckMousePos.onMouseOver += CheckMousePos;
    }
    private void OnDisable()
    {
        _inputSystem.PlayerCookie.GetMoney.Disable();
        _inputSystem.PlayerCookie.GetMoney.performed -= OnClickIncreaseMoney;
        global::CheckMousePos.onMouseOver -= CheckMousePos;
    }
    private void Awake()
    {
      SetUpData();
      _inputSystem = new InputSystem();
    }

    public void AddItemLvl()
    {
        _gameRule.AddItemLvl(Level);
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
    }

    public void OnClickIncreaseMoney(InputAction.CallbackContext context)
    {
        if (mouseEnable)
        {
            _gameRule.IncreaseScore();
            anim.playAnimation();
            clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
        }
    }
    public void NextScene(int index)
    {
        GameManager.Instance.SaveGame();
        GameManager.Instance.NextLevel(index);
    }
    private void CheckMousePos(bool checkMouseEnable)
    {
        mouseEnable = checkMouseEnable;
    }
    private void SetUpData()
    {
        GameManager.Instance.GetGameData();
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
        clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
    }
}
