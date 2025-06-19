using System;
using TMPro;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public static event Action<bool[]> onItemExchange;

    [SerializeField] private TextMeshProUGUI titleInfoText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI costInfoText;


    //This Array holds the items of game : JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3"
    private bool[] itemSlots = new bool[4];

    [Tooltip("This Array holds the cost of game items:\n JumpBoots: 0/ SprintBoots: 1/ SpringSoles: 2/ ClimbGloves: 3")]
    [SerializeField] private float[] itemCosts;
    public bool[] ItemSlots { get => itemSlots; set => itemSlots = value; }

    private Items items = Items.Empty;

    private PlatformerUI platformerUI;

    private void Awake()
    {
        platformerUI = GetComponent<PlatformerUI>();    
    }

    public void JumpBootsSelected()
    {
        items = Items.JumpBoots;
        titleInfoText.text = "Jump Boots";
        infoText.text = "Jump Boots makes jumping free";
        costInfoText.text = "$" + itemCosts[0];
    }

    public void SprintBootsSelected()
    {
        items = Items.SprintBoots;
        titleInfoText.text = "Sprint Boots";
        infoText.text = "Sprint Boots enables sprinting";
        costInfoText.text = "$" + itemCosts[1];

    }

    public void SpringSolesSelected()
    {
        items = Items.SpringSoles;
        titleInfoText.text = "Spring Soles";
        infoText.text = "Permanently increases Jump Height";
        costInfoText.text = "$" + itemCosts[2];

    }

    public void ClimbGlovesSelected()
    {
        items = Items.ClimbGloves;
        titleInfoText.text = "Climb Gloves";
        infoText.text = "Makes Climbing ladders free";
        costInfoText.text = "$" + itemCosts[3];

    }

    public void Buy()
    {
        switch (items)
        {
            case Items.Empty:
                break;
            case Items.JumpBoots:
                if (GameManager.Instance.GetGameData().totalMoney > 0 && itemCosts != null && !itemSlots[0])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - itemCosts[0];
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(itemCosts[0]);
                    }
                    itemSlots[0] = true;
                }
                break;
            case Items.SprintBoots:

                if (GameManager.Instance.GetGameData().totalMoney > 0 && itemCosts != null && !itemSlots[1])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - itemCosts[1];
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(itemCosts[1]);
                    }
                    itemSlots[1] = true;
                }
                break;
            case Items.SpringSoles:
                if (GameManager.Instance.GetGameData().totalMoney > 0 && itemCosts != null && !itemSlots[2])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - itemCosts[2];
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(itemCosts[2]);
                    }
                    itemSlots[2] = true;
                }
                break;
            case Items.ClimbGloves:
                if (GameManager.Instance.GetGameData().totalMoney > 0 && itemCosts != null && !itemSlots[3])
                {
                    //Check if the enouch money to buy the product, if failed give error, if not reduce the total money
                    float newTotalMoney = GameManager.Instance.GetGameData().totalMoney - itemCosts[3];
                    if (newTotalMoney < 0)
                    {
                        Debug.LogError("Not Enough Money");
                    }
                    else
                    {
                        GameManager.Instance.GetGameData().totalMoney = newTotalMoney;
                        platformerUI.UpdateLostMoney(itemCosts[3]);
                    }
                    itemSlots[3] = true;
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