using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUpgrade : MonoBehaviour
{
    public static event Action<int, ClickerItem[]> onItemExchange;

    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Transform parentContext;
    [SerializeField] private float baseCostMultiplier = 1.5f;
    [SerializeField] private ClickerItem[] items = new ClickerItem[0];
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
        CreateUpgradeButtons();
    }

    private void CreateUpgradeButtons()
    {
        upgradeItemInstances = new GameObject[items.Length];
        float cost;
        float money = GameManager.Instance.GetGameData().totalMoney;
        for (int i = 0; i < upgradeItemInstances.Length; i++)
        {
            upgradeItemInstances[i] = Instantiate(upgradeItemPrefab, parentContext.position, Quaternion.identity);
            upgradeItemInstances[i].gameObject.name = "" + i;
            upgradeItemInstances[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = items[i].name;
            upgradeItemInstances[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = items[i].description;
            upgradeItemInstances[i].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "$" + items[i].cost;
            upgradeItemInstances[i].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "" + items[i].tier;

            cost = items[i].cost;
            float calcMoney = money - cost;
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
    }

    private void UpdateUpgradeButtons()
    {
        float cost;
        float money = GameManager.Instance.GetGameData().totalMoney;
        for (int i = 0; i < upgradeItemInstances.Length; i++)
        {
            cost = items[i].cost;
            float calcMoney = money - cost;
            upgradeItemInstances[i].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "$" + items[i].cost;
            upgradeItemInstances[i].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "" + items[i].tier;
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

    }

    public void Buy()
    {
        Debug.Log("Item index: " + clickerIndex);

        float cost = items[clickerIndex].cost;
        float money = GameManager.Instance.GetGameData().totalMoney;
        float calcMoney = money - cost;
        if (calcMoney <= 0)
        {
            Debug.Log("Not Enough Money");
        }
        else
        {
            GameManager.Instance.GetGameData().totalMoney = calcMoney;
            items[clickerIndex].cost = cost * baseCostMultiplier;
            items[clickerIndex].tier++;
            Debug.Log("Bought Item: " + items[clickerIndex].name + "The current tier: " + items[clickerIndex].tier + "\n"
                + "The new Item Cost");
        }

        UpdateUpgradeButtons();
        onItemExchange?.Invoke(clickerIndex, items);
    }


}
