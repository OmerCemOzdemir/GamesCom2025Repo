using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopInfo : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;

    [SerializeField] private Image infoIcon;
    [SerializeField] private TextMeshProUGUI infoTitle;
    [SerializeField] private TextMeshProUGUI infoBody;
    [SerializeField] private TextMeshProUGUI infoCost;


    private void OnEnable()
    {
        PlatformButton.onMouseOverUpgradePlatform += UpdateInfoPanel;
    }

    private void OnDisable()
    {
        PlatformButton.onMouseOverUpgradePlatform -= UpdateInfoPanel;

    }

    private void UpdateInfoPanel(PlatformUpgradeItem upgradeItem, PlatformItemSaveData upgradeItemData, bool enable)
    {
        if (enable)
        {
            //Debug.Log("Upgrade Item: " + upgradeItem.itemName);
            infoTitle.text = "" + upgradeItem.itemName;
            infoBody.text = "" + upgradeItem.itemDescription;
            if (upgradeItem.hasTier)
            {
                infoCost.text = "" + upgradeItemData.cost;
            }
            else
            {
                infoCost.text = "" + upgradeItem.itemCost;
            }


            infoPanel.SetActive(true);
        }
        else
        {
            //Debug.Log("Close the info panel ");
            infoPanel.SetActive(false);

        }
    }



}
