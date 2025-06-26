using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickerManager : MonoBehaviour
{
    [SerializeField] private ClickerUI clickerUI;
    private GameRule gameRule;
    private ClickerEffect effect;
    [SerializeField] private animationManager anim;

    [SerializeField] private int Level;
    [Tooltip("Increase this to longer the time of idle money")]
    [SerializeField] private float baseIdleTime = 1;
    [SerializeField] private float baseActiveTime = 1;
    private bool idleToggle = true;
    private bool activeToggle = true;
    private bool mouseEnable = false;
    private float clickTimer = 0;

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
        effect = transform.GetChild(0).gameObject.GetComponent<ClickerEffect>();
        gameRule = transform.GetChild(1).gameObject.GetComponent<GameRule>();
        _inputSystem = new InputSystem();
    }

    private void FixedUpdate()
    {
        IdleMoney();
    }

    private void IdleMoney()
    {
        if (idleToggle)
        {
            StartCoroutine(IdleClicker());
            idleToggle = false;
        }

        clickTimer += Time.deltaTime + 1;
        Debug.Log("clickTimer: " + clickTimer);
        if (clickTimer > 10)
        {
            StopActiveClicker();
        }
    }

    IEnumerator IdleClicker()
    {
        yield return new WaitForSeconds(baseIdleTime);
        gameRule.IncreaseScore();
        clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
        idleToggle = true;
    }

    private void ActiveMoney()
    {
        if (activeToggle && mouseEnable)
        {
            StartCoroutine(ActiveClicker());
            clickTimer = 0;
            activeToggle = false;
        }
    }

    IEnumerator ActiveClicker()
    {
        yield return new WaitForSeconds(baseActiveTime);
        gameRule.IncreaseScore();
        anim.playAnimation();
        clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();
        effect.IncreaseClickEffect(10);
        activeToggle = true;
    }



    private void StopActiveClicker()
    {
        effect.IncreaseClickEffect(2);
    }

    private void StopActiveClicker(InputAction.CallbackContext context)
    {
        effect.IncreaseClickEffect(2);
    }

    public void AddItemLvl()
    {
        gameRule.AddItemLvl(Level);
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
    }

    public void OnClickIncreaseMoney(InputAction.CallbackContext context)
    {
        ActiveMoney();
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
