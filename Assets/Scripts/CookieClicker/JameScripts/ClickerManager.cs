using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickerManager : MonoBehaviour
{
    public static event Action onActiveClick;
    public static event Action onIdleClick;

    private ClickerEffect effect;

    [SerializeField] private float baseActiveMoneyIncrement = 1; //Defualt is 1
    [SerializeField] private float baseActiveMoneyMultiplier = 1; //Defualt is 1
    [SerializeField] private float baseIdleMoneyIncrement = 0; //Defualt is 0
    [SerializeField] private float baseIdleMoneyMultiplier = 1; //Defualt is 1


    [Tooltip("Increase this to longer the time of idle money")]
    [SerializeField] private float baseIdleTime = 1;
    [SerializeField] private float baseActiveTime = 1;
    private bool idleToggle = true;
    private bool activeToggle = true;
    private bool mouseEnable = false;
    private float clickTimer = 0;



    private InputSystem inputSystem;

    private void OnEnable()
    {
        inputSystem.PlayerCookie.GetMoney.Enable();
        inputSystem.PlayerCookie.GetMoney.performed += OnButtonClick;
        global::CheckMousePos.onMouseOver += CheckMousePos;
        onActiveClick += IncreaseActiveMoney;
        onIdleClick += IncreaseIdleMoney;
        ClickerUpgrade.onItemExchange += ImplementUpgrades;

    }
    private void OnDisable()
    {
        inputSystem.PlayerCookie.GetMoney.Disable();
        inputSystem.PlayerCookie.GetMoney.performed -= OnButtonClick;
        global::CheckMousePos.onMouseOver -= CheckMousePos;
        onActiveClick -= IncreaseActiveMoney;
        onIdleClick -= IncreaseIdleMoney;
        ClickerUpgrade.onItemExchange -= ImplementUpgrades;
    }
    private void Awake()
    {
        SetUpData();
        effect = transform.GetChild(0).gameObject.GetComponent<ClickerEffect>();
        inputSystem = new InputSystem();
    }

    private void FixedUpdate()
    {
        IdleMoney();
    }

    #region Active&IdleLogic

    private void IdleMoney()
    {
        if (idleToggle)
        {
            StartCoroutine(IdleClicker());
            idleToggle = false;
        }

        clickTimer += Time.deltaTime + 1;
        //Debug.Log("clickTimer: " + clickTimer);
        if (clickTimer > 10)
        {
            StopActiveClicker();
        }
    }

    IEnumerator IdleClicker()
    {
        yield return new WaitForSeconds(baseIdleTime);
        onIdleClick?.Invoke();
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
        onActiveClick?.Invoke();
        effect.IncreaseClickEffect(10);
        activeToggle = true;
    }

    private void StopActiveClicker()
    {
        effect.IncreaseClickEffect(2);
    }

    public void OnButtonClick(InputAction.CallbackContext context)
    {
        ActiveMoney();
    }




    #endregion


    private void IncreaseActiveMoney()
    {
        float max = GameManager.Instance.GetGameData().maxTotalMoney;
        float money = GameManager.Instance.GetGameData().totalMoney;

        money += (baseActiveMoneyIncrement * baseActiveMoneyMultiplier);
        if (money >= max)
        {
            money = max;
            GameManager.Instance.GetGameData().totalMoney = money;
        }
        else
        {
            GameManager.Instance.GetGameData().totalMoney = money;
        }

        //Debug.Log("Money: " + GameManager.Instance.GetGameData().totalMoney);
    }

    private void IncreaseIdleMoney()
    {
        float max = GameManager.Instance.GetGameData().maxTotalMoney;
        float money = GameManager.Instance.GetGameData().totalMoney;

        money += (baseIdleMoneyIncrement * baseIdleMoneyMultiplier);
        if (money >= max)
        {
            money = max;
            GameManager.Instance.GetGameData().totalMoney = money;
        }
        else
        {
            GameManager.Instance.GetGameData().totalMoney = money;
        }

        //Debug.Log("Money: " + GameManager.Instance.GetGameData().totalMoney);
    }



    private void ImplementUpgrades(int index, ClickerItem[] items)
    {

        switch (index)
        {
            case 0: //Improve Recipe
                baseActiveMoneyIncrement += (2 * items[index].tier);
                //Debug.Log("baseMoneyIncrement: " + baseActiveMoneyIncrement);
                break;
            case 1: //Better Packing
                baseActiveMoneyMultiplier *= (1.5f * items[index].tier);
                break;
            case 2: //Offshore Cheap Worker
                baseIdleMoneyIncrement += (1.5f + items[index].tier);
                break;
            case 3: //Hire Better Offshore Worker 
                baseIdleMoneyMultiplier *= (0.5f + items[index].tier);
                break;
            case 4: //Better Sales Algorithm
                baseIdleTime = (baseIdleTime / 2);
                break;
            case 5: //Better Delivery/Courier
                baseIdleMoneyIncrement *= (1 + items[index].tier);
                break;
            default:
                break;
        }


    }

    private void UpdateUpgrades(ClickerItem[] items)
    {
        for (int index = 0; index < items.Length; index++)
        {
            switch (index)
            {
                case 0: //Improve Recipe
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseActiveMoneyIncrement += (2 * items[index].tier);
                        PrintFields();
                    }
                    break;
                case 1: //Better Packing
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseActiveMoneyMultiplier *= (1.5f * items[index].tier);
                        PrintFields();

                    }
                    break;
                case 2: //Offshore Cheap Worker
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleMoneyIncrement += (1.5f + items[index].tier);
                        PrintFields();

                    }
                    break;
                case 3: //Hire Better Offshore Worker 
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleMoneyMultiplier *= (0.5f + items[index].tier);
                        PrintFields();

                    }
                    break;
                case 4: //Better Sales Algorithm
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleTime = (baseIdleTime / 2);
                        PrintFields();

                    }
                    break;
                case 5: //Better Delivery/Courier
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleMoneyIncrement *= (1 + items[index].tier);
                        PrintFields();

                    }
                    break;
                default:
                    break;
            }

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

        UpdateUpgrades(GameManager.Instance.GetGameData().clickerItems);
        int walletLevel = GameManager.Instance.GetGameData().walletLevel;
        float maxTotalMoney = 0;
        Debug.Log("max Money: " + walletLevel);
        //This feels stupid.
        switch (walletLevel)
        {
            case 0:
                maxTotalMoney = 10000;
                Debug.Log("max Money: " + maxTotalMoney);
                break;
            case 1:
                maxTotalMoney = 100000;
                Debug.Log("max Money: " + maxTotalMoney);
                break;
            case 2:
                maxTotalMoney = 1000000;
                break;
            default:
                break;
        }

        GameManager.Instance.GetGameData().maxTotalMoney = maxTotalMoney;
        onActiveClick?.Invoke();

    }

    private void PrintFields()
    {
        Debug.Log(" baseActiveMoneyIncrement: " + baseActiveMoneyIncrement + "\n"
    + " baseActiveMoneyMultiplier: " + baseActiveMoneyMultiplier + "\n"
    + " baseIdleMoneyIncrement: " + baseIdleMoneyIncrement + "\n"
    + " baseIdleMoneyMultiplier: " + baseIdleMoneyMultiplier + "\n"
    + " baseIdleTime: " + baseIdleTime + "\n"
    + " baseActiveTime:" + baseActiveTime + "\n"

    );
    }

}


/*
 
    public void AddItemLvl()
    {
        gameRule.AddItemLvl(Level);
    }
 
        clickerUI.currentLevel.text = GameManager.Instance.GetGameData().ItemCount.ToString();
        clickerUI.currentMoney.text = GameManager.Instance.GetGameData().totalMoney.ToString();

    [SerializeField] private float baseActiveMoneyIncrement = 1; //Defualt is 1
    [SerializeField] private float baseActiveMoneyMultiplier = 1; //Defualt is 1
    [SerializeField] private float baseIdleMoneyIncrement = 0; //Defualt is 0
    [SerializeField] private float baseIdleMoneyMultiplier = 1; //Defualt is 1


    [Tooltip("Increase this to longer the time of idle money")]
    [SerializeField] private float baseIdleTime = 1;
    [SerializeField] private float baseActiveTime = 1;
 */