using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUpgrade : MonoBehaviour
{
    public static event Action<int, ClickerItemSaveData[], List<UpgradeItem>> onItemExchange;

    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Transform parentContext;
    private ClickerItemSaveData[] itemsData = new ClickerItemSaveData[0];
    //private UpgradeItem[] upgradeItems;
    private List<UpgradeItem> upgradeItems = new List<UpgradeItem>();
    private GameObject[] upgradeItemInstances;
    private int clickerIndex = 0;

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

    private void InitilizeScriptableObjects()
    {
        //Assets/ScriptableObjects/ClickerItems
        string[] files;
        files = Directory.GetFiles("Assets/ScriptableObjects/ClickerItems");
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].EndsWith(".meta"))
            {
                upgradeItems.Add(AssetDatabase.LoadAssetAtPath<UpgradeItem>(files[i]));
                //Debug.Log("Test: " + files[i]);
            }

        }

        foreach (var item in upgradeItems)
        {
            Debug.Log("Test: " + item.name);
        }

    }


    private void CreateUpgradeButtons()
    {
        upgradeItemInstances = new GameObject[upgradeItems.Count];

        float cost;
        double money = GameManager.Instance.GetGameData().totalMoney;
        for (int i = 0; i < upgradeItemInstances.Length; i++)
        {
            upgradeItemInstances[i] = Instantiate(upgradeItemPrefab, parentContext.position, Quaternion.identity);
            upgradeItemInstances[i].gameObject.name = "" + i;
            upgradeItemInstances[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = upgradeItems[i].itemName;
            upgradeItemInstances[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = upgradeItems[i].itemDescription;

            if (itemsData[i].tier == 0)
            {
                upgradeItemInstances[i].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "$" + upgradeItems[i].baseItemCost;
            }
            else
            {
                upgradeItemInstances[i].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "$" + itemsData[i].cost;
            }

            upgradeItemInstances[i].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "" + itemsData[i].tier;
            upgradeItemInstances[i].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = upgradeItems[i].itemIcon;

            cost = itemsData[i].cost;
            double calcMoney = money - cost;
            if (calcMoney <= 0)
            {
                upgradeItemInstances[i].GetComponent<Button>().interactable = false;
                upgradeItemInstances[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                upgradeItemInstances[i].GetComponent<Button>().interactable = true;
                upgradeItemInstances[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }

            upgradeItemInstances[i].transform.SetParent(parentContext);
            upgradeItemInstances[i].GetComponent<RectTransform>().localScale = Vector3.one;

        }


        for (int j = 0; j < upgradeItemInstances.Length; j++)
        {
            if (!itemsData[j].unlock)
            {
                upgradeItemInstances[j].SetActive(false);
            }
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
            if (calcMoney <= 0)
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


        for (int j = 0; j < upgradeItemInstances.Length; j++)
        {
            if (itemsData[j].unlock)
            {
                upgradeItemInstances[j].SetActive(true);
            }

            int maxTier = upgradeItems[j].maxTier;
            int currentTier = itemsData[j].tier;
            bool maxTierReached = maxTier <= currentTier;
            if (maxTierReached)
            {
                upgradeItemInstances[j].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                upgradeItemInstances[j].GetComponent<Button>().interactable = false;

            }

        }



    }

    public void Buy()
    {
        Debug.Log("Item index: " + clickerIndex);

        float cost = itemsData[clickerIndex].cost;
        double money = GameManager.Instance.GetGameData().totalMoney;
        double calcMoney = money - cost;
        //Debug.Log("maxTierReached: " + maxTierReached);
        if (calcMoney <= 0)
        {
            Debug.Log("Not Enough Money Or Already fully Upgraded");
        }
        else
        {
            GameManager.Instance.GetGameData().totalMoney = calcMoney;
            cost = itemsData[clickerIndex].cost * upgradeItems[clickerIndex].costMultiplier;
            cost = (float)Math.Round(cost);
            itemsData[clickerIndex].cost = cost;
            Debug.Log("Cost: " + cost + "Exponent: " + upgradeItems[clickerIndex].costMultiplier);
            itemsData[clickerIndex].tier++;
            Debug.Log("Bought Item: " + upgradeItems[clickerIndex].itemName + "The current tier: " + itemsData[clickerIndex].tier + "\n"
                + "The new Item Cost");
        }

        UpdateUpgradeButtons();
        onItemExchange?.Invoke(clickerIndex, itemsData, upgradeItems);
        GameManager.Instance.GetGameData().clickerItems = itemsData;

    }

    private void SetUpData()
    {
        //Debug.Log("New Game: " + GameManager.Instance.GetGameData().newGame);
        if (GameManager.Instance.GetGameData().newGame)
        {
            itemsData = new ClickerItemSaveData[upgradeItems.Count];
            Debug.Log("New Game: " + GameManager.Instance.GetGameData().newGame);


            for (int i = 0; i < itemsData.Length; i++)
            {
                Debug.Log("itemsData: " + i + ": " + upgradeItems[i].baseItemCost);
                itemsData[i] = new ClickerItemSaveData();
                itemsData[i].ID = upgradeItems[i].name;
                itemsData[i].cost = upgradeItems[i].baseItemCost;
                itemsData[i].tier = 0;
                itemsData[i].unlock = upgradeItems[i].itemUnlocked;
            }
            //PrintArr(items);

            GameManager.Instance.GetGameData().clickerItems = itemsData;
            GameManager.Instance.SaveGame();
            GameManager.Instance.GetGameData().newGame = false;
        }
        else
        {
            itemsData = GameManager.Instance.GetGameData().clickerItems;
        }

    }

    private void PrintArr(ClickerItemSaveData[] arr)
    {
        foreach (var item in arr)
        {
            Debug.Log("item: " + item.cost);
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