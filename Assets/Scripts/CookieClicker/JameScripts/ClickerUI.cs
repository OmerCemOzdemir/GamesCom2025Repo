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

    private void UpdateText()
    {
        currentMoney.text = GameManager.Instance.GetGameData().maxTotalMoney + " / $" + GameManager.Instance.GetGameData().totalMoney;
    }

}
