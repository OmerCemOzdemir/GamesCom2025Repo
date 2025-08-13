using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerInfo : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;

    [SerializeField] private Image infoIcon;
    [SerializeField] private TextMeshProUGUI infoTitle;
    [SerializeField] private TextMeshProUGUI infoBody;


    private void OnEnable()
    {
        ClickerButton.onMouseOverUpgrade += UpdateInfoPanel;
    }

    private void OnDisable()
    {
        ClickerButton.onMouseOverUpgrade -= UpdateInfoPanel;

    }

    private void UpdateInfoPanel(ClickerUpgradeItem upgradeItem, bool enable)
    {
        if (enable)
        {
            //Debug.Log("Upgrade Item: " + upgradeItem.itemName);
            infoIcon.sprite = upgradeItem.itemIcon;
            infoTitle.text = upgradeItem.itemName;
            infoBody.text = upgradeItem.itemDescription;

            infoPanel.SetActive(true);
        }
        else
        {
            //Debug.Log("Close the info panel ");
            infoPanel.SetActive(false);

        }
    }



}
