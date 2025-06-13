
using TMPro;
using UnityEngine;

public class PlatformerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "$" + GameManager.Instance.GetGameData().totalMoney;
    }


    private void OnEnable()
    {
        PlatformerManager.onMoneyChange += UpdateMoneyText;
    }

    private void OnDisable()
    {
        PlatformerManager.onMoneyChange -= UpdateMoneyText;

    }

}
