using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUpgrade : MonoBehaviour
{
    public static event Action<int, ClickerItemSaveData[], List<ClickerUpgradeItem>> onItemExchange;

    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Transform parentContext;
    
    private ClickerItemSaveData[] itemsData = new ClickerItemSaveData[0];
    //private UpgradeItem[] upgradeItems;
    [HideInInspector] public List<ClickerUpgradeItem> upgradeItems = new List<ClickerUpgradeItem>();
    private GameObject[] upgradeItemInstances;
    private int clickerIndex = 0;
    //private int buttonPosY = -110;

    public int ClickerIndex { get => clickerIndex; set => clickerIndex = value; }

    private void OnEnable()
    {
        ClickerManager.onIdleClick += UpdateUpgradeButtons;
    }

    private void OnDisable()
    {
        ClickerManager.onIdleClick -= UpdateUpgradeButtons;
    }

    private void Awake()
    {
        InitilizeScriptableObjects();
        SetUpData();
        CreateUpgradeButtons();

    }

    private void FixedUpdate()
    {
        UpdateUpgradeButtons();
    }

    private void InitilizeScriptableObjects()
    {
        // Get upgrades from UpgradeManager
        if (UpgradeManager.Instance != null)
        {
            upgradeItems = new List<ClickerUpgradeItem>(
                UpgradeManager.Instance.GetClickerItems()
            );
        }

        else
        {
            // fallback if UpgradeManager is missing
            upgradeItems = new List<ClickerUpgradeItem>(
                Resources.LoadAll<ClickerUpgradeItem>("ClickerItems")
            );
        }

    }

    private void CreateUpgradeButtons()
    {
        int count = Mathf.Min(upgradeItems.Count, itemsData.Length);
        upgradeItemInstances = new GameObject[count];

        double money = GameManager.Instance.GetGameData().totalMoney;

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(upgradeItemPrefab, parentContext.position, Quaternion.identity);
            go.name = i.ToString();
            go.transform.SetParent(parentContext, false);

            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = upgradeItems[i].itemName;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = upgradeItems[i].itemDescription;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "$" + itemsData[i].cost;
            go.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "" + itemsData[i].tier;

            int safeTier = Mathf.Clamp(itemsData[i].tier, 0, upgradeItems[i].itemIcon.Length - 1);
            go.transform.GetChild(4).GetComponent<Image>().sprite = upgradeItems[i].itemIcon[safeTier];

            bool afford = money - itemsData[i].cost > 0;
            var btn = go.GetComponent<Button>();
            var img = go.GetComponent<Image>();
            btn.interactable = afford && itemsData[i].unlock;
            img.color = itemsData[i].unlock ? (afford ? Color.white : new Color(1,1,1,0.5f))
                                            : new Color(1,0,0,0.5f);

            upgradeItemInstances[i] = go;
        }

        if (count > 0)
        {
            float h = upgradeItemInstances[0].GetComponent<RectTransform>().rect.height * count + 50f;
            float w = parentContext.GetComponent<RectTransform>().rect.width;
            parentContext.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
        }

    }

    private void UpdateUpgradeButtons()
    {
        float cost;
        double money = GameManager.Instance.GetGameData().totalMoney;

        for (int i = 0; i < upgradeItemInstances.Length; i++)
        {
            cost = itemsData[i].cost;
            double calcMoney = money - cost;
            upgradeItemInstances[i].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "$" + itemsData[i].cost;
            upgradeItemInstances[i].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "" + itemsData[i].tier;
            upgradeItemInstances[i].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = upgradeItems[i].itemIcon[itemsData[i].tier];
            //Debug.Log("Calc Money : " + calcMoney);
            if (itemsData[i].unlock)
            {
                if (money < cost)
                {
                    upgradeItemInstances[i].GetComponent<Button>().interactable = false;
                    upgradeItemInstances[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    upgradeItemInstances[i].GetComponent<Button>().interactable = true;
                    upgradeItemInstances[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
            }

        }


        for (int j = 0; j < upgradeItemInstances.Length; j++)
        {

            int maxTier = upgradeItems[j].maxTier;
            int currentTier = itemsData[j].tier;
            //Debug.Log("currentTier: " + currentTier);
            currentTier++;
            bool maxTierReached = maxTier <= currentTier;
            if (maxTierReached)
            {
                upgradeItemInstances[j].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                upgradeItemInstances[j].GetComponent<Button>().interactable = false;
                upgradeItemInstances[j].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "Max";
            }
            //Debug.Log(" after currentTier: " + currentTier);

        }

    }

    public void Buy()
    {
        Debug.Log("Item index: " + clickerIndex);

        float cost = itemsData[clickerIndex].cost;
        double money = GameManager.Instance.GetGameData().totalMoney;
        double calcMoney = money - cost;
        int maxTier = upgradeItems[clickerIndex].maxTier;
        int currentTier = itemsData[clickerIndex].tier;
        currentTier++;
        //Debug.Log("upgradeItemsData[clickerIndex].tier: " + itemsData[clickerIndex].tier);
        //Debug.Log("currentTier: " + currentTier);
        bool maxTierReached = maxTier <= currentTier;
        //Debug.Log("maxTierReached: " + maxTierReached);
        if (calcMoney <= 0 || maxTierReached)
        {
            Debug.Log("Not Enough Money Or Already fully Upgraded");
        }
        else
        {
            GameManager.Instance.GetGameData().totalMoney = calcMoney;
            itemsData[clickerIndex].tier++;
            cost = upgradeItems[clickerIndex].itemTiers[itemsData[clickerIndex].tier].tierCost;
            cost = (float)Math.Round(cost);
            itemsData[clickerIndex].cost = cost;
            //Debug.Log("Cost: " + cost + "Exponent: " + upgradeItems[clickerIndex].itemTiers);
            Debug.Log("Bought Item: " + upgradeItems[clickerIndex].itemName + "The current tier: " + itemsData[clickerIndex].tier + "\n"
                + "The new Item Cost");
        }

        UpdateUpgradeButtons();
        onItemExchange?.Invoke(clickerIndex, itemsData, upgradeItems);
        GameManager.Instance.GetGameData().clickerItems = itemsData;

    }

    private void SetUpData()
    {
        var gameData = GameManager.Instance.GetGameData();

        if (gameData.clickerUpgradeNewGame || gameData.clickerItems == null || gameData.clickerItems.Length != upgradeItems.Count)
        {
            // (Re)build save to match the current catalog
            itemsData = new ClickerItemSaveData[upgradeItems.Count];
            for (int i = 0; i < itemsData.Length; i++)
            {
                itemsData[i] = new ClickerItemSaveData
                {
                    ID     = upgradeItems[i].name,
                    cost   = upgradeItems[i].itemTiers[0].tierCost,
                    tier   = 0,
                    unlock = upgradeItems[i].itemUnlocked
                };
            }
            gameData.clickerItems = itemsData;
            gameData.clickerUpgradeNewGame = false;
            GameManager.Instance.SaveGame();
        }
        else
        {
            itemsData = gameData.clickerItems;
        }
    }

    private void PrintArr(ClickerItemSaveData[] arr)
    {
        foreach (var item in arr)
        {
            Debug.Log("item ID: " + item.ID);
            Debug.Log("item Cost: " + item.cost);
            Debug.Log("item Tier: " + item.tier);
            Debug.Log("item Unlock: " + item.unlock);

        }
    }

}


/*
             if (items[clickerIndex].tier == 0)
            {
                newCostMultiplier[clickerIndex] = (float)Math.Pow(cost, baseCostMultiplier);
                items[clickerIndex].cost = newCostMultiplier[clickerIndex];
            }
            else
            {
                newCostMultiplier[clickerIndex] = (float)Math.Pow(cost, newCostMultiplier[clickerIndex]);
                items[clickerIndex].cost = newCostMultiplier[clickerIndex];
            }


#if UNITY_EDITOR

[CustomEditor(typeof(ClickerUpgrade))]
public class ClickerUpgradeCustomInspector : Editor
{
    SerializedProperty items;

    private void OnEnable()
    {
        items = serializedObject.FindProperty("items");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(items);

        serializedObject.ApplyModifiedProperties();
    }


}


#endif

            cost = (float)Math.Pow(cost, baseCostMultiplier);


            //Debug.Log("upgradeItems.Count: " + upgradeItems.Count);
            //Debug.Log("items.Length: " + items.Length);
           // Debug.Log("items: " + 1 + ": " + upgradeItems[1].baseItemCost);


 */