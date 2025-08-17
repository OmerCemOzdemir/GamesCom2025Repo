using TMPro;
using UnityEngine;

public class ClickerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentMoney;


    private void OnEnable()
    {
        ClickerManager.onActiveClick += UpdateText;
        ClickerManager.onIdleClick += UpdateText;
    }

    private void OnDisable()
    {
        ClickerManager.onActiveClick -= UpdateText;
        ClickerManager.onIdleClick -= UpdateText;

    }

    private void FixedUpdate()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        double money = GameManager.Instance.GetGameData().totalMoney;
        double maxMoney = GameManager.Instance.GetGameData().maxTotalMoney;

        if (money >= maxMoney)
        {
            currentMoney.color = Color.red;
            currentMoney.text = "! $" + GameManager.Instance.GetGameData().totalMoney;
        }
        else
        {
            currentMoney.color = Color.green;
            currentMoney.text = "$" + GameManager.Instance.GetGameData().totalMoney;
        }

    }

}
