using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainHubUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Transform itemsPanel;
    [SerializeField] private GameObject itemIconPrefab;
    private GameObject[] itemIconArr;
    private PlatformUpgradeShop platformUpgradeShop;

    private void OnEnable()
    {
        PlatformerManager.onMoneyChange += UpdateMoneyText;
    }

    private void OnDisable()
    {
        PlatformerManager.onMoneyChange -= UpdateMoneyText;
    }

    private void Awake()
    {
        UpdateMoneyText(0);
        platformUpgradeShop = GetComponent<PlatformUpgradeShop>();
    }

    private void Start()
    {
        UpdateItemIcons();
    }

    public void UpdateMoneyText(float money)
    {
        moneyText.text = "$" + GameManager.Instance.GetGameData().totalMoney;
    }


    private void DestroyItemIcons()
    {

        if (itemsPanel.childCount != 0)
        {
            for (int i = 0; i < itemsPanel.childCount; i++)
            {
                Debug.Log("Help");
                Destroy(itemsPanel.GetChild(i).gameObject);
            }
        }
    }


    public void UpdateItemIcons()
    {
        DestroyItemIcons();
        PlatformItemSaveData[] itemSaveData = GameManager.Instance.GetGameData().platformItems;
        itemIconArr = new GameObject[itemSaveData.Length];
        for (int i = 0; i < itemSaveData.Length; i++)
        {
            itemIconArr[i] = Instantiate(itemIconPrefab);
            itemIconArr[i].transform.SetParent(itemsPanel);

            if (itemSaveData[i].unlock)
            {
                itemIconArr[i].GetComponent<Image>().sprite = platformUpgradeShop.UpgradeItems[i].itemIcon;
            }
            else
            {
                itemIconArr[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            //itemIcon.GetComponent<Image>().sprite = 
        }

    }

}
