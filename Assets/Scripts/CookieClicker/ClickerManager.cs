using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class ClickerManager : MonoBehaviour
{
    public static event Action onActiveClick;
    public static event Action onIdleClick;
    public static event Action<SFX> onPlaySFX;
    public static event Action<Music> onPlayMusic;

    //private ClickerEffect effect;
    private List<ClickerUpgradeItem> upgradeClickerItems = new List<ClickerUpgradeItem>();
    private List<PlatformUpgradeItem> upgradePlatformItems = new List<PlatformUpgradeItem>();

    [Header("Parameters")]
    [SerializeField] private float baseActiveMoneyIncrement = 1; //Default is 1
    [SerializeField] private float baseActiveMoneyMultiplier = 1; //Default is 1
    [SerializeField] private float baseIdleMoneyIncrement = 1; //Default is 1
    [SerializeField] private float baseIdleMoneyMultiplier = 1; //Default is 1
    [Tooltip("Increase this to longer the elevatorSpeed of idle money")]
    [SerializeField] private float baseIdleTime = 1;
    [SerializeField] private float baseActiveTime = 1;
    [Space(10)]

    [Header("Animation and Effect")]
    [SerializeField] private Animator playerOfficeAnimator;
    [SerializeField] private Animator playerClickEffectAnimator;
    [SerializeField] private Animator AFKAnimator1;
    [SerializeField] private Animator AFKAnimator2;
    [SerializeField] private Animator AFKAnimator3;
    [SerializeField] private Animator AFKAnimator4;

    [SerializeField] private AnimationClip AFKWalkClip1;
    [SerializeField] private AnimationClip AFKWalkClip2;
    [SerializeField] private AnimationClip AFKWalkClip3;
    [SerializeField] private AnimationClip AFKWalkClip4;


    [SerializeField] private float speedInterval = 1;
    [SerializeField] private ClickerEffects effects;
    [SerializeField] private AFKEffectManager AFKEffectManager;
    [SerializeField] private float[] AFKRateIntervals;

    private int clickSpeed = 1000; //Bigger the number slower the speed
    private float idleMoneyProfitRate = 0;
    private bool[] AFKBools = { true, true, true, true };


    private float activeMoneyIncrement;
    private float activeMoneyMultiplier;
    private float idleMoneyIncrement;
    private float idleMoneyMultiplier;
    private float idleTime;
    private bool isMouseOverButton = false;

    private bool idleToggle = true;
    private bool activeToggle = true;
    private bool gameToggle = true;
    private bool mouseEnable = false;
    private bool animToggleOnce = true;
    private bool animToggleOfficePlayer = true;
    private bool enableIdleMoney = false;

    private float clickTimer = 0;
    private float clickAnimTimer = 0;
    private int currentFrame = 0;
    private int previousFrame = 0;

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
        //effect = transform.GetChild(0).gameObject.GetComponent<ClickerEffect>();
        inputSystem = new InputSystem();
        //effect.StartEffect();
        //effect.IncreaseClickEffect(0);
        SetClickingEnabled(true);
        //idleMoneyInitialProfit = ((idleMoneyIncrement + 1) * idleMoneyMultiplier);
    }

    private void Start()
    {
        //InvokeRepeating(nameof(AccumulateMoney), 2.0f, 1f);
        InvokeRepeating(nameof(CalculateMoneyPerSec), 5f, 1f);
    }

    private void FixedUpdate()
    {
        if (gameToggle && enableIdleMoney)
        {
            IdleMoney();
        }
    }

    private void Update()
    {
        //HandleAnimSpeed();
        clickAnimTimer += 6 * Time.deltaTime;
        //Debug.Log("clickAnimTimer: " + clickAnimTimer);
        if (clickAnimTimer > 3)
        {
            animToggleOfficePlayer = true;
            playerOfficeAnimator.SetTrigger("Idle");
            //currentFrame = Time.frameCount;
            //clickSpeed = (currentFrame - previousFrame);
            //clickSpeed = 1000;
            clickAnimTimer = 0;

        }

    }

    #endregion

    #region Active&IdleLogic


    private void HandleAFKAnim()
    {
        // profit increase formula: ((y-x)/x)*100
        //idleMoneyProfitPercentage = (((idleMoneyIncrement * idleMoneyMultiplier) - idleMoneyInitialProfit) / idleMoneyInitialProfit) * 100;

        if (idleMoneyProfitRate > 0 && idleMoneyProfitRate < AFKRateIntervals[0])
        {
            if (AFKBools[0])
            {
                Debug.Log("The Profit rate: AFK_1");
                PlayerAFKAnim(AFKAnimator1, AFKWalkClip1);
                AFKBools[0] = false;
            }
        }
        else if (idleMoneyProfitRate >= AFKRateIntervals[1] && idleMoneyProfitRate < AFKRateIntervals[2])
        {
            if (AFKBools[1])
            {
                Debug.Log("The Profit rate: AFK_2");
                PlayerAFKAnim(AFKAnimator2, AFKWalkClip2);
                AFKBools[1] = false;
            }
        }
        else if (idleMoneyProfitRate > AFKRateIntervals[2])
        {
            if (AFKBools[2])
            {
                Debug.Log("The Profit rate: AFK_3");
                PlayerAFKAnim(AFKAnimator3, AFKWalkClip3);
                AFKBools[2] = false;
            }
        }

        //59000
        //Debug.Log("The Profit rate: " + idleMoneyProfitPercentage);
    }

    private void PlayerAFKAnim(Animator anim, AnimationClip clip)
    {
        anim.SetTrigger("Walk");
        StartCoroutine(DelayOnAFKAnim(anim, clip));

    }

    IEnumerator DelayOnAFKAnim(Animator anim, AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        anim.SetTrigger("Work");
        AFKEffectManager.ActivateEffects();

    }


    private void CalculateMoneyPerSec()
    {
        //idleMoneyProfitRatePre = idleMoneyProfitRate;
        Debug.Log("Profit: " + idleMoneyProfitRate);
        idleMoneyProfitRate = 0;
    }

    private void IdleMoney()
    {
        if (idleToggle)
        {
            StartCoroutine(IdleClicker());
            //effect.StartEffect();
            //effect.IncreaseClickEffect(2);
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
        idleMoneyProfitRate += idleMoneyIncrement * idleMoneyMultiplier;
        AFKEffectManager.SpawnEffects();
        HandleAFKAnim();
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
        if (animToggleOfficePlayer)
        {
            playerOfficeAnimator.SetTrigger("Working");
            animToggleOfficePlayer = false;
        }
        playerClickEffectAnimator.SetTrigger("Click");
        effects.SpawnParticle();
        clickAnimTimer = 0;
        //previousFrame = currentFrame;
        //currentFrame = Time.frameCount;
        clickSpeed = (currentFrame - previousFrame);
        if (animToggleOnce)
        {
            animToggleOnce = false;
        }
        Debug.Log("Clicker Speed: " + clickSpeed);
        onActiveClick?.Invoke();
        //Debug.Log("Money: " + GameManager.Instance.GetGameData().totalMoney);
        yield return new WaitForSeconds(baseActiveTime);
        onPlaySFX?.Invoke(SFX.Active);
        //effect.IncreaseClickEffect(10);
        activeToggle = true;
    }

    private void StopActiveClicker()
    {
        //effect.IncreaseClickEffect(0);
    }

    public void OnButtonClick(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse Enable: " + mouseEnable);
        ActiveMoney();
    }

    public void SetClickingEnabled(bool enabled)
    {
        mouseEnable = enabled;


        // if (effect != null && effect.TryGetComponent(out Animator anim))
        //     anim.enabled = enabled;
    }

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
        FindAnyObjectByType<ClickerUI>().UpdateText();
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

    #endregion

    #region Upgrades

    private void ImplementUpgrades(int index, ClickerItemSaveData[] itemsData, List<ClickerUpgradeItem> upgradeItems)
    {
        //ResetFields(upgradeItems[index].itemEffector);
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
                if (itemsData[index].tier > 0) { enableIdleMoney = true; }
                break;
            case ClickerItemEffetors.baseIdleMoneyIncrement:
                idleMoneyIncrement = ImplementOperations(index, itemsData, upgradeItems, idleMoneyIncrement);
                if (itemsData[index].tier > 0) { enableIdleMoney = true; }
                break;
            case ClickerItemEffetors.baseIdleTime:
                idleTime = ImplementOperations(index, itemsData, upgradeItems, idleTime);
                if (itemsData[index].tier > 0) { enableIdleMoney = true; }
                break;
            case ClickerItemEffetors.walletLevel:
                //Debug.Log("Wallet Level: " + GameManager.Instance.GetGameData().walletLevel);
                GameManager.Instance.GetGameData().walletLevel = itemsData[index].tier; //(int)ImplementOperations(index, itemsData, upgradeItems, idleTime);
                //Debug.Log("Wallet Level: " + GameManager.Instance.GetGameData().walletLevel);
                CalculateWallet();
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

    #endregion

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

    public void EnableInputs()
    {
        inputSystem.PlayerCookie.GetMoney.Enable();
        gameToggle = true;
        //effect.StartEffect();

    }

    public void DisableInputs()
    {
        inputSystem.PlayerCookie.GetMoney.Disable();
        gameToggle = false;
        //effect.PauseEffect();

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

    public void CalculateWallet()
    {
        //GameManager.Instance.GetGameData().walletLevel = walletLevel;
        int walletLevel = GameManager.Instance.GetGameData().walletLevel;
        float maxTotalMoney = 10000;
        for (int i = 0; i < walletLevel; i++)
        {
            maxTotalMoney *= 10;
        }
        //Debug.Log("Wallet Level: " + walletLevel);
        //Debug.Log("Max Money: " + maxTotalMoney);
        GameManager.Instance.GetGameData().maxTotalMoney = maxTotalMoney;

    }

    public void SetUpData()
    {
        GameManager.Instance.GetGameData();

        //UpdateUpgrades(GameManager.Instance.GetGameData().clickerItems);

        //Debug.Log("max Money: " + walletLevel);
        ResetFields();
        onPlayMusic?.Invoke(Music.Clicker);

        UpdateUpgrades(GameManager.Instance.GetGameData().clickerItems, upgradeClickerItems);
        CalculateWallet();

        //onActiveClick?.Invoke();
        FindAnyObjectByType<ClickerUI>().UpdateText();

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
    + " idleTime: " + idleTime + "\n"
    + " idle profit rate: " + idleMoneyProfitRate + "\n";
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


        if (clicksPerTick >= speedInterval)
        {
            clickSpeed++;
            clicksPerTick = 0;
            speedInterval += 1;
        }
        else
        {
            if (speedInterval != 10)
            {
                speedInterval--;
            }

            if (clickSpeed != 0)
            {
                clickSpeed--;
            }
        }

    IEnumerator StopAnimationSlowly()
    {

        yield return new WaitForSeconds(backgroundAnimator.speed);
        effect.IncreaseClickEffect(0);
        animToggle = true;

    }
    IEnumerator AnimationSlowly(float length)
    {
        Debug.Log("Animation len: " + length);
        yield return new WaitForSeconds(backgroundAnimator.speed);
        animToggle = true;
    }

            backgroundAnimator.SetBool("StartAnim_2", true);
            backgroundAnimator.SetBool("StartAnim_1", false);
            backgroundAnimator.SetBool("StopAnim", false);


    //backgroundAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>1 && !backgroundAnimator.IsInTransition(0)
    //!backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim)

    public IEnumerator CheckAnimationCompleted(string CurrentAnim, Action Oncomplete)
    {
        //Debug.Log("outside the while loop CheckAnimation");
        //StopAnimationSignal();
        while (!backgroundAnimator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim))
        {
            Debug.Log("inside the while loop CheckAnimation");
            yield return null;
        }
        Oncomplete?.Invoke();
    }

    private void HandleAnimSpeed()
    {
        clickAnimTimer += 4 * Time.deltaTime;
        //Debug.Log("clickAnimTimer: " + clickAnimTimer);
        if (clickAnimTimer > 10)
        {
            //currentFrame = Time.frameCount;
            //clickSpeed = (currentFrame - previousFrame);
            //clickSpeed = 1000;
            clickAnimTimer = 0;
            playerOfficeAnimator.SetTrigger("Idle");
        }

        if (animToggle)
        {
            //Debug.Log("Click Speed: " + clickSpeed);
            //StartAnimation(clickSpeed);
            //StartCoroutine(TestRoutine(10));

            //Debug.Log("Animation Parameters: " + " clickSpeed: " + clickSpeed + " clickAnimTimer: " + clickAnimTimer + " animToggle: " + animToggle);
            //Debug.Log("Animation Toggle: " + animToggle + "Speed: " + clickSpeed);
            animToggle = false;
        }

    }

    private IEnumerator CheckAnimation(float timer, Action Oncomplete)
    {
        //Debug.Log("Animation is running");
        yield return new WaitForSeconds(timer);
        animToggle = true;
        Oncomplete?.Invoke();
    }

    private void StartAnimation(int speed)
    {
        //Debug.Log("Animation Toggle: " + animToggle + "Speed: " + speed);
        if (speed <= 100)
        {
            Debug.Log("Current Click Speed: " + clickSpeed + " ClickerAnimSpeed_2");

            //SetAnim_1
            playerOfficeAnimator.SetTrigger("SetAnim_2");
            StartCoroutine(CheckAnimation(backgroundAnimClipSpeed2.length, () =>
            {
                animToggle = true;
            }));
            //StartCoroutine(AnimationSlowly(backgroundAnimClipSleep2.length));
        }
        else if (101 < speed && speed <= 800)
        {
            Debug.Log("Current Click Speed: " + clickSpeed + " ClickerAnimSpeed_1");
            playerOfficeAnimator.SetTrigger("SetAnim_1");

            StartCoroutine(CheckAnimation(backgroundAnimClipSpeed1.length, () =>
            {
                animToggle = true;
            }));
            //StartCoroutine(AnimationSlowly(backgroundAnimClipSleep1.length));

        }
        else if (speed >= 900)
        {
            Debug.Log("Current Click Speed: " + clickSpeed + " ClickerAnimSpeed_0");
            playerOfficeAnimator.SetTrigger("SetAnim_0");
            StartCoroutine(CheckAnimation(backgroundAnimClipSpeed0.length, () =>
            {
                animToggle = true;
            }));
        }

    }


 */