using TMPro;
using UnityEngine;

public class ClickerUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private string[] upgradeItemName;
    [SerializeField] private Transform parentContext;
    private GameObject[] upgradeItemInstances;

    private void Awake()
    {
        upgradeItemInstances = new GameObject[upgradeItemName.Length];

        for (int i = 0; i < upgradeItemName.Length; i++)
        {
            upgradeItemInstances[i] = Instantiate(upgradeItemPrefab, parentContext.position, Quaternion.identity);
            upgradeItemInstances[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = upgradeItemName[i];
            upgradeItemInstances[i].transform.SetParent(parentContext);
        }
    }

}
