using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClickerManager : MonoBehaviour
{
    public static event Action onActiveClick;
    public static event Action onIdleClick;
    public static event Action<SFX> onPlaySFX;
    public static event Action<Music> onPlayMusic;

    private ClickerEffect effect;
    private List<ClickerUpgradeItem> upgradeClickerItems = new List<ClickerUpgradeItem>();
    private List<PlatformUpgradeItem> upgradePlatformItems = new List<PlatformUpgradeItem>();


    [SerializeField] private float baseActiveMoneyIncrement = 1; //Default is 1
    [SerializeField] private float baseActiveMoneyMultiplier = 1; //Default is 1
    [SerializeField] private float baseIdleMoneyIncrement = 0; //Default is 0
    [SerializeField] private float baseIdleMoneyMultiplier = 1; //Default is 1


    private float activeMoneyIncrement;
    private float activeMoneyMultiplier;
    private float idleMoneyIncrement;
    private float idleMoneyMultiplier;
    private float idleTime;
    private bool isMouseOverButton = false;


    [Tooltip("Increase this to longer the elevatorSpeed of idle money")]
    [SerializeField] private float baseIdleTime = 1;
    [SerializeField] private float baseActiveTime = 1;
    private bool idleToggle = true;
    private bool activeToggle = true;
    private bool gameToggle = true;
    private bool mouseEnable = false;
    private float clickTimer = 0;

    private InputSystem inputSystem;

    #region UnityFunctions

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
        InitilizeScriptableObjects();
        SetUpData();
        effect = transform.GetChild(0).gameObject.GetComponent<ClickerEffect>();
        inputSystem = new InputSystem();

    }

    private void FixedUpdate()
    {
        if (gameToggle)
        {
            IdleMoney();
        }
    }

    #endregion


    #region Active&IdleLogic

    private void IdleMoney()
    {
        if (idleToggle)
        {
            StartCoroutine(IdleClicker());
            effect.StartEffect();
            effect.IncreaseClickEffect(2);
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
        onIdleClick?.Invoke();
        yield return new WaitForSeconds(idleTime);
        onPlaySFX?.Invoke(SFX.Idle);
        idleToggle = true;
    }

    public void ActiveMoney()
    {
        if (activeToggle && mouseEnable && isMouseOverButton)
        {
            StartCoroutine(ActiveClicker());
            clickTimer = 0;
            activeToggle = false;
        }
    }

    IEnumerator ActiveClicker()
    {
        onActiveClick?.Invoke();
        Debug.Log("Money: " + GameManager.Instance.GetGameData().totalMoney);
        yield return new WaitForSeconds(baseActiveTime);
        onPlaySFX?.Invoke(SFX.Active);
        effect.IncreaseClickEffect(10);
        activeToggle = true;
    }

    private void StopActiveClicker()
    {
        effect.IncreaseClickEffect(2);
    }

    public void OnButtonClick(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse Enable: " + mouseEnable);
        ActiveMoney();
    }

    public void SetClickingEnabled(bool enabled)
    {
        mouseEnable = enabled;

        if (effect != null && effect.TryGetComponent(out Animator anim))
            anim.enabled = enabled;
    }


    #endregion


    private void IncreaseActiveMoney()
    {
        double max = GameManager.Instance.GetGameData().maxTotalMoney;
        double money = GameManager.Instance.GetGameData().totalMoney;

        money += (activeMoneyIncrement * activeMoneyMultiplier);
        if (money >= max)
        {
            money = max;
            //Debug.Log("money: " + money);

            GameManager.Instance.GetGameData().totalMoney = money;
        }
        else
        {
            money = (float)Math.Round(money);
            GameManager.Instance.GetGameData().totalMoney = money;
        }
        FindAnyObjectByType<ClickerUI>().SetUpFirst();
        //Debug.Log("Money: " + GameManager.Instance.GetGameData().totalMoney);
    }

    private void IncreaseIdleMoney()
    {
        double max = GameManager.Instance.GetGameData().maxTotalMoney;
        double money = GameManager.Instance.GetGameData().totalMoney;

        money += (idleMoneyIncrement * idleMoneyMultiplier);
        if (money >= max)
        {
            money = max;
            money = (float)Math.Round(money);
            GameManager.Instance.GetGameData().totalMoney = money;
        }
        else
        {
            money = (float)Math.Round(money);
            GameManager.Instance.GetGameData().totalMoney = money;
        }

        //Debug.Log("Money: " + GameManager.Instance.GetGameData().totalMoney);
    }

    private void ImplementUpgrades(int index, ClickerItemSaveData[] itemsData, List<ClickerUpgradeItem> upgradeItems)
    {
        ResetFields(upgradeItems[index].itemEffector);
        switch (upgradeItems[index].itemEffector)
        {
            case ClickerItemEffetors.baseActiveMoneyIncrement:
                activeMoneyIncrement = ImplementOperations(index, itemsData, upgradeItems, activeMoneyIncrement);
                break;
            case ClickerItemEffetors.baseActiveMoneyMultiplier:
                activeMoneyMultiplier = ImplementOperations(index, itemsData, upgradeItems, activeMoneyMultiplier);
                break;
            case ClickerItemEffetors.baseIdleMoneyMultiplier:
                idleMoneyMultiplier = ImplementOperations(index, itemsData, upgradeItems, idleMoneyMultiplier);
                break;
            case ClickerItemEffetors.baseIdleMoneyIncrement:
                idleMoneyIncrement = ImplementOperations(index, itemsData, upgradeItems, idleMoneyIncrement);
                break;
            case ClickerItemEffetors.baseIdleTime:
                idleTime = ImplementOperations(index, itemsData, upgradeItems, idleTime);
                break;
            default:
                break;

        }
    }

    private float ImplementOperations(int index, ClickerItemSaveData[] itemsData, List<ClickerUpgradeItem> upgradeItems, float fieldEffected)
    {
        if (itemsData[index].tier != 0)
        {
            switch (upgradeItems[index].itemOperationOnItemEffector)
            {
                case Operations.Add:
                    fieldEffected += upgradeItems[index].itemTiers[itemsData[index].tier].tierEffect;
                    break;
                case Operations.Multiply:
                    fieldEffected *= upgradeItems[index].itemTiers[itemsData[index].tier].tierEffect;
                    break;
                case Operations.Subtract:
                    fieldEffected -= upgradeItems[index].itemTiers[itemsData[index].tier].tierEffect;
                    break;
                case Operations.Divide:
                    fieldEffected /= upgradeItems[index].itemTiers[itemsData[index].tier].tierEffect;
                    break;
                default:
                    break;
            }
        }

        return fieldEffected;
    }

    private void UpdateUpgrades(ClickerItemSaveData[] itemsData, List<ClickerUpgradeItem> upgradeItems)
    {
        for (int i = 0; i < itemsData.Length; i++)
        {
            ImplementUpgrades(i, itemsData, upgradeItems);
        }
    }

    public void NextScene(int index)
    {
        GameManager.Instance.GetGameData().checkpointEnable = false;
        GameManager.Instance.SaveGame();
        GameManager.Instance.NextLevel(index);
    }

    private void CheckMousePos(bool isHover)
    {
        isMouseOverButton = isHover;
    }


    private void InitilizeScriptableObjects()
    {
        //Assets/ScriptableObjects/ClickerItems
        string[] files;
        files = Directory.GetFiles("Assets/ScriptableObjects/ClickerItems");
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".meta"))
            {
                upgradeClickerItems.Add(AssetDatabase.LoadAssetAtPath<ClickerUpgradeItem>(files[i]));
                //Debug.Log("Test: " + files[i]);
            }

        }

        files = Directory.GetFiles("Assets/ScriptableObjects/PlatformItems");
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".meta"))
            {
                upgradePlatformItems.Add(AssetDatabase.LoadAssetAtPath<PlatformUpgradeItem>(files[i]));
                //Debug.Log("Test: " + files[i]);
            }

        }

    }

    public void EnableInputs()
    {
        inputSystem.PlayerCookie.GetMoney.Enable();
        gameToggle = true;
        effect.StartEffect();

    }

    public void DisableInputs()
    {
        inputSystem.PlayerCookie.GetMoney.Disable();
        gameToggle = false;
        effect.StopEffect();

    }

    private void ResetFields()
    {
        activeMoneyIncrement = baseActiveMoneyIncrement;
        activeMoneyMultiplier = baseActiveMoneyMultiplier;
        idleMoneyIncrement = baseIdleMoneyIncrement;
        idleMoneyMultiplier = baseIdleMoneyMultiplier;
        idleTime = baseIdleTime;
    }

    private void ResetFields(ClickerItemEffetors field)
    {
        switch (field)
        {
            case ClickerItemEffetors.baseActiveMoneyIncrement:
                activeMoneyIncrement = baseActiveMoneyIncrement;

                break;
            case ClickerItemEffetors.baseActiveMoneyMultiplier:
                activeMoneyMultiplier = baseActiveMoneyMultiplier;

                break;
            case ClickerItemEffetors.baseIdleMoneyMultiplier:
                idleMoneyIncrement = baseIdleMoneyIncrement;

                break;
            case ClickerItemEffetors.baseIdleMoneyIncrement:
                idleMoneyMultiplier = baseIdleMoneyMultiplier;

                break;
            case ClickerItemEffetors.baseIdleTime:
                idleTime = baseIdleTime;

                break;
            default:
                break;

        }

    }

    public void SetUpData()
    {
        GameManager.Instance.GetGameData();

        //UpdateUpgrades(GameManager.Instance.GetGameData().clickerItems);
        int walletLevel = GameManager.Instance.GetGameData().walletLevel;
        float maxTotalMoney = 0;
        //Debug.Log("max Money: " + walletLevel);
        ResetFields();
        onPlayMusic?.Invoke(Music.Clicker);
        UpdateUpgrades(GameManager.Instance.GetGameData().clickerItems, upgradeClickerItems);

        maxTotalMoney = 10000;
        for (int i = 0; i < walletLevel; i++)
        {
            maxTotalMoney *= 10;
        }
        GameManager.Instance.GetGameData().maxTotalMoney = maxTotalMoney;
        //onActiveClick?.Invoke();
        FindAnyObjectByType<ClickerUI>().SetUpFirst();

        PlatformItemSaveData[] platformItemData = GameManager.Instance.GetGameData().platformItems;
        for (int i = 0; i < platformItemData.Length; i++)
        {
            switch (upgradePlatformItems[i].itemType)
            {
                case PlatformItemType.Permanent:
                    break;
                case PlatformItemType.Temporary:
                    GameManager.Instance.GetGameData().platformItems[i].unlock = false;
                    if (upgradePlatformItems[i].hasTier)
                    {
                        GameManager.Instance.GetGameData().platformItems[i].cost = upgradePlatformItems[i].costTiers[0];
                        GameManager.Instance.GetGameData().platformItems[i].tier = 0;
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().platformItems[i].cost = upgradePlatformItems[i].itemCost;
                    }
                    break;
                case PlatformItemType.RepeatPurchase:
                    break;
            }
        }

    }

    public string PrintFields()
    {
        string text = " activeMoneyIncrement: " + activeMoneyIncrement + "\n"
    + " activeMoneyMultiplier: " + activeMoneyMultiplier + "\n"
    + " idleMoneyIncrement: " + idleMoneyIncrement + "\n"
    + " idleMoneyMultiplier: " + idleMoneyMultiplier + "\n"
    + " idleTime: " + idleTime + "\n";

        Debug.Log(text);

        return text;
    }



}


/*


        ClickerItemSaveData[] clickerItemSaveData = GameManager.Instance.GetGameData().clickerItems;
        foreach (var item in clickerItemSaveData)
        {
            Debug.Log("item: " + item.ID);
            Debug.Log("item: " + item.unlock);
        }





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


        for (int index = 0; index < items.Length; index++)
        {
            switch (index)
            {
                case 0: //Improve Recipe
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseActiveMoneyIncrement += (2 * items[index].tier);
                        //PrintFields();
                    }
                    break;
                case 1: //Better Packing
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseActiveMoneyMultiplier *= (1.5f * items[index].tier);
                        //PrintFields();

                    }
                    break;
                case 2: //Offshore Cheap Worker
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleMoneyIncrement += (1.5f + items[index].tier);
                        //PrintFields();

                    }
                    break;
                case 3: //Hire Better Offshore Worker 
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleMoneyMultiplier *= (0.5f + items[index].tier);
                        //PrintFields();

                    }
                    break;
                case 4: //Better Sales Algorithm
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleTime = (baseIdleTime / 2);
                        //PrintFields();

                    }
                    break;
                case 5: //Better Delivery/Courier
                    for (int i = 0; i < items[index].tier; i++)
                    {
                        baseIdleMoneyIncrement *= (1 + items[index].tier);
                        //PrintFields();

                    }
                    break;
                default:
                    break;
            }

        }

         //This feels stupid.
        switch (walletLevel)
        {
            case 0:
                maxTotalMoney = 10000;
                //Debug.Log("max Money: " + maxTotalMoney);
                break;
            case 1:
                maxTotalMoney = 100000;
                //Debug.Log("max Money: " + maxTotalMoney);
                break;
            case 2:
                maxTotalMoney = 1000000;
                break;
            default:
                break;
        }



 */