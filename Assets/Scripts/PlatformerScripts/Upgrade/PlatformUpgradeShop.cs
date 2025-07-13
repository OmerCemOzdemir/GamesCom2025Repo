using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlatformUpgradeShop : MonoBehaviour
{
    public static event Action<bool[]> onItemExchange;
    public static event Action onNewGame;

    [SerializeField] private GameObject upgradeShopUI;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private TextMeshProUGUI titleInfoText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI costInfoText;
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Transform parentContext;

    private PlatformItemSaveData[] upgradeItemsData;
    private List<PlatformUpgradeItem> upgradeItems = new List<PlatformUpgradeItem>();
    private GameObject[] upgradeItemInstances;
    private int platformItemIndex = 0;

    public int PlatformItemIndex { get => platformItemIndex; set => platformItemIndex = value; }
    public List<PlatformUpgradeItem> UpgradeItems { get => upgradeItems; set => upgradeItems = value; }

    private MainHubUI mainHubUI;

    private void OnEnable()
    {
        TestScript.onDataChange += SetUpData;
        PlayerControler.onPlayerOpenShop += OpenShop;
    }

    private void OnDisable()
    {
        TestScript.onDataChange -= SetUpData;
        PlayerControler.onPlayerOpenShop -= OpenShop;
    }

    private void Awake()
    {
        mainHubUI = GetComponent<MainHubUI>();
        InitilizeScriptableObjects();

    }


    private void Start()
    {
        SetUpData();
        CreateUpgradeButtons();
    }

    private void InitilizeScriptableObjects()
    {
        //Assets/ScriptableObjects/PlatformItems
        string[] files;
        files = Directory.GetFiles("Assets/ScriptableObjects/PlatformItems");
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".meta"))
            {
                upgradeItems.Add(AssetDatabase.LoadAssetAtPath<PlatformUpgradeItem>(files[i]));
                //Debug.Log("Test: " + files[i]);
            }

        }

        foreach (var item in upgradeItems)
        {
            //Debug.Log("Test: " + item.name);
        }

    }

    private void SetUpData()
    {
        //Debug.Log("CAll ondatachange: " + GameManager.Instance.GetGameData().platformItems[1]);

        if (GameManager.Instance.GetGameData().platformNewGame)
        {
            upgradeItemsData = new PlatformItemSaveData[upgradeItems.Count];
            onNewGame?.Invoke();
            for (int i = 0; i < upgradeItemsData.Length; i++)
            {
                upgradeItemsData[i] = new PlatformItemSaveData();
                if (upgradeItems[i].hasTier)
                {
                    upgradeItemsData[i].cost = upgradeItems[i].costTiers[0];
                }
                else
                {
                    upgradeItemsData[i].cost = upgradeItems[i].itemCost;

                }
                upgradeItemsData[i].tier = 0;
                upgradeItemsData[i].unlock = false;
                Debug.Log("upgradeItemsData: " + i + ": " + upgradeItemsData[i].cost);
            }

            GameManager.Instance.GetGameData().platformItems = upgradeItemsData;
            GameManager.Instance.GetGameData().platformNewGame = false;
        }
        else
        {
            upgradeItemsData = GameManager.Instance.GetGameData().platformItems;

        }
        //PrintArr();
    }


    private void CreateUpgradeButtons()
    {
        upgradeItemInstances = new GameObject[upgradeItems.Count];

        double money = GameManager.Instance.GetGameData().totalMoney;
        for (int i = 0; i < upgradeItemInstances.Length; i++)
        {
            upgradeItemInstances[i] = Instantiate(upgradeItemPrefab, parentContext.position, Quaternion.identity);
            upgradeItemInstances[i].transform.SetParent(parentContext.transform);
            upgradeItemInstances[i].gameObject.name = "" + i;
            upgradeItemInstances[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = upgradeItems[i].itemIcon;
            if (upgradeItems[i].hasTier)
            {
                upgradeItemInstances[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                upgradeItemInstances[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            //Debug.Log("upgradeItemsData: " + i + ": " + upgradeItemsData[i].cost);

            UpdateItemButtons(i);
        }

    }

    public void UpdateShopTexts()
    {
        Debug.Log("UpdateShopTexts: " + platformItemIndex);
        titleInfoText.text = "" + upgradeItems[platformItemIndex].itemName;
        infoText.text = "" + upgradeItems[platformItemIndex].itemDescription;
        if (upgradeItems[platformItemIndex].hasTier)
        {
            costInfoText.text = "" + upgradeItemsData[platformItemIndex].cost;
        }
        else
        {
            costInfoText.text = "" + upgradeItems[platformItemIndex].itemCost;
        }

        double money = GameManager.Instance.GetGameData().totalMoney;
        double cost = (double)upgradeItemsData[platformItemIndex].cost;
        double calc = money - cost;
        bool isBuyable = calc < 0;
        if (isBuyable)
        {
            buyButton.GetComponent<Button>().enabled = false;
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            buyButton.GetComponent<Button>().enabled = true;
            buyButton.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }

    }



    private void UpdateItemButtons()
    {

        if (upgradeItems[platformItemIndex].hasTier)
        {
            int maxTier = upgradeItems[platformItemIndex].effectTiersPercentage.Length;
            int tier = upgradeItemsData[platformItemIndex].tier;
            tier++;
            upgradeItemInstances[platformItemIndex].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "" + tier;
            if (maxTier == tier)
            {
                Debug.Log("Max Tier Reached");
                upgradeItemInstances[platformItemIndex].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 1f);
                upgradeItemInstances[platformItemIndex].GetComponent<Button>().enabled = false;
                platformItemIndex = -1;
            }

        }
        else
        {
            if (upgradeItemsData[platformItemIndex].unlock)
            {
                upgradeItemInstances[platformItemIndex].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 1f);
                upgradeItemInstances[platformItemIndex].GetComponent<Button>().enabled = false;
            }
            else
            {
                upgradeItemInstances[platformItemIndex].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                upgradeItemInstances[platformItemIndex].GetComponent<Button>().enabled = true;
            }
        }
    }



    private void UpdateItemButtons(int index)
    {

        if (upgradeItems[index].hasTier)
        {
            int maxTier = upgradeItems[index].effectTiersPercentage.Length;
            int tier = upgradeItemsData[index].tier;
            tier++;
            upgradeItemInstances[index].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "" + tier;
            if (maxTier == tier)
            {
                Debug.Log("Max Tier Reached");
                upgradeItemInstances[index].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 1f);
                upgradeItemInstances[index].GetComponent<Button>().enabled = false;
                index = -1;
            }
        }
        else
        {
            if (upgradeItemsData[index].unlock)
            {
                upgradeItemInstances[index].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 1f);
                upgradeItemInstances[index].GetComponent<Button>().enabled = false;
            }
            else
            {
                upgradeItemInstances[index].transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                upgradeItemInstances[index].GetComponent<Button>().enabled = true;
            }
        }
    }



    public void Buy()
    {
        if (platformItemIndex == -1)
        {

        }
        else
        {
            double money = GameManager.Instance.GetGameData().totalMoney;
            double cost = (double)upgradeItemsData[platformItemIndex].cost;
            double calc = money - cost;
            if (calc < 0)
            {
                Debug.Log("Can not Purchase");
            }
            else
            {
                if (upgradeItems[platformItemIndex].hasTier)
                {
                    int maxTier = upgradeItems[platformItemIndex].effectTiersPercentage.Length;
                    int tier = upgradeItemsData[platformItemIndex].tier;
                    tier++;
                    Debug.Log("Max Tier: " + maxTier + " Tier: " + tier);
                    if (upgradeItemsData[platformItemIndex].unlock)
                    {
                        if (maxTier < tier)
                        {
                            Debug.Log("Max Tier Reached");
                        }
                        else
                        {
                            tier--;
                            upgradeItemsData[platformItemIndex].tier++;
                            upgradeItemsData[platformItemIndex].cost = upgradeItems[platformItemIndex].costTiers[tier];
                            GameManager.Instance.GetGameData().totalMoney = calc;
                        }
                    }
                    else
                    {
                        upgradeItemsData[platformItemIndex].tier = 0;
                        tier = upgradeItemsData[platformItemIndex].tier;
                        upgradeItemsData[platformItemIndex].cost = upgradeItems[platformItemIndex].costTiers[tier];
                        upgradeItemsData[platformItemIndex].unlock = true;
                        GameManager.Instance.GetGameData().totalMoney = calc;
                    }
                }
                else
                {
                    upgradeItemsData[platformItemIndex].tier = 1;
                    upgradeItemsData[platformItemIndex].unlock = true;
                    GameManager.Instance.GetGameData().totalMoney = calc;

                }
                mainHubUI.UpdateMoneyText(0);
                UpdateShopTexts();
                UpdateItemButtons();
                mainHubUI.UpdateItemIcons();
                //GameManager.Instance.SaveGame();
                //PrintArr();
            }
        }

    }


    public void PrintArr()
    {
        foreach (var item in upgradeItemsData)
        {
            Debug.Log("upgradeItemsData item.cost: " + item.cost);
            Debug.Log("upgradeItemsData item.tier: " + item.tier);
            Debug.Log("upgradeItemsData item.unlock: " + item.unlock);
        }
    }


    public void OpenShop() { upgradeShopUI.SetActive(true); }

    public void CloseShop() { upgradeShopUI.SetActive(false); }

}



/*
 * 

        PlatformItemType type = upgradeItems[platformItemIndex].itemType;
        switch (type)
        {
            case PlatformItemType.Permanent:

                break;
            case PlatformItemType.Temporary:

                break;
            case PlatformItemType.RepeatPurchase:

                break;
            default:
                break;
        }
 * 
 * 
 * 
 * 
 * 
 * 

    [Tooltip("This Array holds the cost of game upgradeItemsData:\n JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3")]
    [SerializeField] private PlatformItem[] platformItems;
 * 
 * 
    //This Array holds the items of game : JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
    private bool[] itemSlots = new bool[4];
    public bool[] ItemSlots { get => itemSlots; set => itemSlots = value; }
    private int itemIndex = 0;

    private Items items = Items.Empty;

    private PlatformerUI platformerUI;

 *         onItemExchange?.Invoke(itemSlots);

 * 
 * 
        foreach (var item in GameManager.Instance.GetGameData().platformItems)
        {
            //Debug.Log("GameData in UpgradeShop: " + item);
        }
        Debug.Log("GameData in PlatformUpgradeShop: " + GameManager.Instance.GetGameData().totalMoney); 


    private void ChangeIndex()
    {
        switch (items)
        {
            case Items.Empty:
                break;
            case Items.JumpBoots:
                itemIndex = 0;
                break;
            case Items.SprintBoots:
                itemIndex = 1;
                break;
            case Items.SpringSoles:
                itemIndex = 2;
                break;
            case Items.ClimbGloves:
                itemIndex = 3;
                break;
            default:
                break;
        }
    }

    public void JumpBootsSelected()
    {
        items = Items.JumpBoots;
        ChangeIndex();
        titleInfoText.text = platformItems[itemIndex].name;
        infoText.text = platformItems[itemIndex].description;
        costInfoText.text = "$" + platformItems[itemIndex].cost;
    }

    public void SprintBootsSelected()
    {
        items = Items.SprintBoots;
        ChangeIndex();
        titleInfoText.text = platformItems[itemIndex].name;
        infoText.text = platformItems[itemIndex].description;
        costInfoText.text = "$" + platformItems[itemIndex].cost;
    }

    public void SpringSolesSelected()
    {
        items = Items.SpringSoles;
        ChangeIndex();
        titleInfoText.text = platformItems[itemIndex].name;
        infoText.text = platformItems[itemIndex].description;
        costInfoText.text = "$" + platformItems[itemIndex].cost;
    }

    public void ClimbGlovesSelected()
    {
        items = Items.ClimbGloves;
        ChangeIndex();
        titleInfoText.text = platformItems[itemIndex].name;
        infoText.text = platformItems[itemIndex].description;
        costInfoText.text = "$" + platformItems[itemIndex].cost;

    }

    public void Buy()
    {
        switch (items)
        {
            case Items.Empty:
                break;
            case Items.JumpBoots:
                if (GameManager.Instance.GetGameData().totalMoney > 0 && platformItems != null && !itemSlots[0])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    double newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(platformItems[itemIndex].cost);
                        itemSlots[itemIndex] = true;
                    }
                }
                break;
            case Items.SprintBoots:

                if (GameManager.Instance.GetGameData().totalMoney > 0 && platformItems != null && !itemSlots[1])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    double newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(platformItems[itemIndex].cost);
                        itemSlots[itemIndex] = true;
                    }
                }
                break;
            case Items.SpringSoles:
                if (GameManager.Instance.GetGameData().totalMoney > 0 && platformItems != null && !itemSlots[2])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    double newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(platformItems[itemIndex].cost);
                        itemSlots[itemIndex] = true;
                    }
                }
                break;
            case Items.ClimbGloves:
                if (GameManager.Instance.GetGameData().totalMoney > 0 && platformItems != null && !itemSlots[3])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    double newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(platformItems[itemIndex].cost);
                        itemSlots[itemIndex] = true;
                    }
                }
                break;
            default:
                break;
        }
        onItemExchange?.Invoke(itemSlots);
    }


public enum Items
{
    JumpBoots,
    SprintBoots,
    SpringSoles,
    ClimbGloves,
    Empty
}
 */