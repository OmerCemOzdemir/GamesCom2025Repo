using TMPro;
using UnityEngine;

public class ClickerUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private Transform parentContext;
    [SerializeField] private ClickerItem[] items = new ClickerItem[0];  
    private GameObject[] upgradeItemInstances;

    private void Awake()
    {
        CreateUpgradeButtons();
    }

    private void CreateUpgradeButtons()
    {
        upgradeItemInstances = new GameObject[items.Length];
        for (int i = 0; i < upgradeItemInstances.Length; i++)
        {
            upgradeItemInstances[i] = Instantiate(upgradeItemPrefab, parentContext.position, Quaternion.identity);
            upgradeItemInstances[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = items[i].name;
            upgradeItemInstances[i].transform.SetParent(parentContext);
            upgradeItemInstances[i].GetComponent<RectTransform>().localScale = Vector3.one;

            //
        }
    }

}
