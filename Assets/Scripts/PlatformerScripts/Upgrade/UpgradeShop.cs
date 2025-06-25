using System;
using TMPro;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public static event Action<bool[]> onItemExchange;

    [SerializeField] private TextMeshProUGUI titleInfoText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI costInfoText;

    [Tooltip("This Array holds the cost of game items:\n JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3")]
    [SerializeField] private PlatformItem[] platformItems;


    //This Array holds the items of game : JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
    private bool[] itemSlots = new bool[4];
    public bool[] ItemSlots { get => itemSlots; set => itemSlots = value; }
    private int itemIndex = 0;

    private Items items = Items.Empty;

    private PlatformerUI platformerUI;

    private void OnEnable()
    {
        TestScript.onDataChange += LoadItemData;
    }

    private void OnDisable()
    {
        TestScript.onDataChange -= LoadItemData;

    }

    private void Awake()
    {
        platformerUI = GetComponent<PlatformerUI>();
        itemSlots = GameManager.Instance.GetGameData().platformItems;
        onItemExchange?.Invoke(itemSlots);

    }

    private void LoadItemData()
    {
        itemSlots = GameManager.Instance.GetGameData().platformItems;
        //Debug.Log("CAll ondatachange: " + GameManager.Instance.GetGameData().platformItems[1]);
        onItemExchange?.Invoke(itemSlots);
        foreach (var item in GameManager.Instance.GetGameData().platformItems)
        {
           //Debug.Log("GameData in UpgradeShop: " + item);
        }
        Debug.Log("GameData in UpgradeShop: " + GameManager.Instance.GetGameData().totalMoney);
       
    }

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
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
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
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
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
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
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
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - platformItems[itemIndex].cost;
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

}

public enum Items
{
    JumpBoots,
    SprintBoots,
    SpringSoles,
    ClimbGloves,
    Empty
}