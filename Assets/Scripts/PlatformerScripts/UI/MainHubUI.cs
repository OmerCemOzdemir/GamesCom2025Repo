using TMPro;
using UnityEngine;

public class MainHubUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

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
    }



    public void UpdateMoneyText(float money)
    {
        moneyText.text = "$" + GameManager.Instance.GetGameData().totalMoney;
    }

}
